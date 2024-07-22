using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

[System.Serializable]
public class EnvironmentCameraSettings
{

    [Header("Zoom")]
    public float MaxDistance = 30;
    public float DefaultDistance = 20;
    public float MinDistance = 10;

    public float ZoomSpeed = 75;

    public float ZoomDuration = 1f;

    [Header("Rotation")]
    public float RotationStep = 90;
    public float RotationDuration = 0.35f;

    [Header("Movement")]
    public bool AllowEdgeScroll = false;
    public float CameraSpeed = 15f;
    public float ScreenEdgeDelta = 1;
}

public class CameraRig : MonoBehaviour
{
    [Header("MainCamera")]
    [SerializeField] private CinemachineCamera CM_Main;

    [Header("Settings")] //later there will be a separate rig for encounters and home base
    [SerializeField] private EnvironmentCameraSettings EnvironmentCameraSettings;

    //singleton
    private static CameraRig _instance;
    public static CameraRig Instance { get { return _instance; } }

    private CameraFollowTarget _followTarget;
    private CinemachinePositionComposer _composer;
    private CameraBounds _bounds;
    private bool _rotating = false;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }

        Build();
    }

    private void Build()
    {
        _bounds = this.gameObject.GetComponentInChildren<CameraBounds>();
        _followTarget = this.gameObject.GetComponentInChildren<CameraFollowTarget>();

        //TODO
        //_composer = CM_Main.GetCinemachineComponent<CinemachinePositionComposer>();
        //_composer.m_CameraDistance = EnvironmentCameraSettings.DefaultDistance;
        //_composer.m_MinimumDistance = EnvironmentCameraSettings.MinDistance;
        //_composer.m_MaximumDistance = EnvironmentCameraSettings.MaxDistance;
    }

    private void FixedUpdate()
    {
        Update_Zoom();
        Update_Rotation();
        Update_Movement_Keys();
        Update_Movement_Mouse();
    }

    public float GoBetween(CharacterComponent caster, CharacterComponent target)
    {
        Vector3 midwayPoint = Vector3.Lerp(caster.GetWorldLocation(), target.GetWorldLocation(), 0.5f);

        GoTo(midwayPoint);

        return ZoomOut();
    }

    public void GoTo(Vector3 location)
    {
        Unfollow();
        _followTarget.transform.position = location;
    }

    public void Follow(CharacterComponent character)
    {
        Debug.Log("New Follow Target " + character.GetID());
      
        _followTarget.AttachToCharacter(character);
    }

    public void Unfollow()
    {
        _followTarget.Release();
    }

    public float ZoomOut()
    {
        StopCoroutine(Coroutine_PerformZoom_Out());

        StartCoroutine(Coroutine_PerformZoom_Out());

        return EnvironmentCameraSettings.ZoomDuration;
    }

    private void Update_Zoom()
    {
        Vector2 mouseScrollDelta = Input.mouseScrollDelta;

        if (mouseScrollDelta.magnitude > 0)
        {
            float currentDistance = 0;//_composer.m_CameraDistance;

            float targetDistance = EnvironmentCameraSettings.MinDistance;

            if (mouseScrollDelta.y < 0) { targetDistance = EnvironmentCameraSettings.MaxDistance; }

            currentDistance = Mathf.Lerp(currentDistance, targetDistance, Time.smoothDeltaTime * EnvironmentCameraSettings.ZoomSpeed);

            // _composer.m_CameraDistance = currentDistance;
        }
    }

    private void Update_Rotation()
    {
        if (_rotating) { return; }

        if (Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(Coroutine_RotateCamera(true));
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            StartCoroutine(Coroutine_RotateCamera(false));
        }
    }

    private void Update_Movement_Keys()
    {
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            Vector3 direction = Vector3.zero;
            direction.x = Input.GetAxis("Horizontal");
            direction.z = Input.GetAxis("Vertical");

            Update_Movement(direction);
        }
    }

    private void Update_Movement_Mouse()
    {
        Vector3 direction = Vector3.zero;

        if (EnvironmentCameraSettings.AllowEdgeScroll)
        {
            if (Input.mousePosition.x >= Screen.width - EnvironmentCameraSettings.ScreenEdgeDelta)
            {
                direction.x = 1;
            }
            else if (Input.mousePosition.x <= 0 + EnvironmentCameraSettings.ScreenEdgeDelta)
            {
                direction.x = -1;
            }

            if (Input.mousePosition.y >= Screen.height - EnvironmentCameraSettings.ScreenEdgeDelta)
            {
                direction.z = 1;
            }
            else if (Input.mousePosition.y <= 0 + EnvironmentCameraSettings.ScreenEdgeDelta)
            {
                direction.z = -1;
            }
        }

        Update_Movement(direction);
    }

    private void Update_Movement(Vector3 direction)
    {
        direction *= EnvironmentCameraSettings.CameraSpeed;

        //camera forward and right vectors:
        Vector3 forward = Camera.main.transform.forward;
        forward.y = 0;

        Vector3 right = Camera.main.transform.right;

        direction = forward * direction.z + right * direction.x;

        Vector3 origin = _followTarget.transform.position;
        Vector3 destination = _followTarget.transform.position + direction;
        Vector3 lerpedPosition = Vector3.Lerp(origin, destination, Time.smoothDeltaTime);

        if (_bounds.Contains(lerpedPosition))
        {
            _followTarget.transform.position = lerpedPosition;
        }
        else
        {
            lerpedPosition = Vector3.Lerp(origin, _bounds.ClosestPointOnBounds(destination), Time.smoothDeltaTime);
            _followTarget.transform.position = lerpedPosition;
        }
    }

    private IEnumerator Coroutine_RotateCamera(bool clockwise)
    {
        _rotating = true;

        float originAngle = CM_Main.transform.localEulerAngles.y;
        float step = EnvironmentCameraSettings.RotationStep;

        if (!clockwise) step *= -1;

        float targetAngle = originAngle + step;

        float currentTime = 0;

        while (currentTime < EnvironmentCameraSettings.RotationDuration)
        {
            float currentAngle = Mathf.LerpAngle(originAngle, targetAngle, currentTime / EnvironmentCameraSettings.RotationDuration);
            CM_Main.transform.localEulerAngles = new Vector3(45, currentAngle, 0);

            currentTime += Time.smoothDeltaTime;

            yield return new WaitForSecondsRealtime(Time.smoothDeltaTime);
        }

        yield return null;

        CM_Main.transform.localEulerAngles = new Vector3(45, targetAngle, 0);

        _rotating = false;
    }

    private IEnumerator Coroutine_PerformZoom_Out()
    {
        float time = 0;

        float initialDistance = 0;//_composer.m_CameraDistance;

        float targetDistance = EnvironmentCameraSettings.MaxDistance;

        //while (time < EnvironmentCameraSettings.ZoomDuration)
        //{
        //    _composer.m_CameraDistance = Mathf.Lerp(initialDistance, targetDistance, time / EnvironmentCameraSettings.ZoomDuration);

        //    time += Time.smoothDeltaTime;

        //    yield return new WaitForFixedUpdate();
        //}

        //_composer.m_CameraDistance = targetDistance;

        yield return null;
    }

    public static bool IsActive()
    {
        return Instance != null;
    }

    public static void Launch()
    {
        if(!IsActive())
        {
            Instantiate<GameObject>(ResourceUtil.GetEncounterCameraRig());
        }
    }

}
