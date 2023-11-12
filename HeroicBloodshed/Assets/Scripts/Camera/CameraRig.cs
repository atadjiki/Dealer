using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnvironmentCameraSettings
{

    [Header("Zoom")]
    public float MaxDistance = 30;
    public float DefaultDistance = 20;
    public float MinDistance = 10;

    public float ZoomSpeed = 75;

    [Header("Rotation")]
    public float RotationStep = 90;
    public float RotationDuration = 0.35f;
    
    [Header("Movement")]
    public float CameraSpeed = 15f;
    public float ScreenEdgeDelta = 1;
}

[RequireComponent(typeof(BoxCollider))]
public class CameraRig : MonoBehaviour
{
    [Header("MainCamera")]
    [SerializeField] private CinemachineVirtualCamera CM_Main;
    [SerializeField] private CinemachineVirtualCamera CM_Overview;

    [Header("FollowTarget")]
    [SerializeField] private CameraFollowTarget FollowTarget;

    [Header("Settings")] //later there will be a separate rig for encounters and home base
    [SerializeField] private EnvironmentCameraSettings EnvironmentCameraSettings;

    //singleton
    private static CameraRig _instance;
    public static CameraRig Instance { get { return _instance; } }

    private CinemachineFramingTransposer _framingTransposer;
    private BoxCollider _collider;
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
        _collider = this.gameObject.GetComponent<BoxCollider>();

        _framingTransposer = CM_Main.AddCinemachineComponent<CinemachineFramingTransposer>();
        _framingTransposer.m_CameraDistance = EnvironmentCameraSettings.DefaultDistance;
        _framingTransposer.m_MinimumDistance = EnvironmentCameraSettings.MinDistance;
        _framingTransposer.m_MaximumDistance = EnvironmentCameraSettings.MaxDistance;

        SwitchToOverviewCam();
    }

    private void Update()
    {
        Update_Zoom();
        Update_Rotation();
        Update_Movement_Keys();
        Update_Movement_Mouse();
    }

    public void Follow(CharacterComponent character)
    {
        Debug.Log("New Follow Target " + character.GetID());
        SwitchToMainCam();
        FollowTarget.AttachToCharacter(character);
    }

    public void Unfollow()
    {
        SwitchToMainCam();
        FollowTarget.Release();
    }

    private void Update_Zoom()
    {
        Vector2 mouseScrollDelta = Input.mouseScrollDelta;

        if (mouseScrollDelta.magnitude > 0)
        {
            float currentDistance =_framingTransposer.m_CameraDistance;

            float targetDistance = EnvironmentCameraSettings.MinDistance;

            if(mouseScrollDelta.y < 0) { targetDistance = EnvironmentCameraSettings.MaxDistance; }

            currentDistance = Mathf.Lerp(currentDistance, targetDistance, Time.smoothDeltaTime * EnvironmentCameraSettings.ZoomSpeed);

            _framingTransposer.m_CameraDistance = currentDistance;
        }
    }

    private void Update_Rotation()
    {
        if (_rotating) { return; }

        if(Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(Coroutine_RotateCamera(true));
        }
        else if(Input.GetKeyDown(KeyCode.Q))
        {
            StartCoroutine(Coroutine_RotateCamera(false));
        }
    }

    private void Update_Movement_Keys()
    {
        if(Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
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

        Vector3 origin = FollowTarget.transform.position;
        Vector3 destination = FollowTarget.transform.position + direction;
        Vector3 lerpedPosition = Vector3.Lerp(origin, destination, Time.smoothDeltaTime);

        if (_collider.bounds.Contains(lerpedPosition))
        {
            FollowTarget.transform.position = lerpedPosition;
        }
        else
        {
            lerpedPosition = Vector3.Lerp(origin, _collider.ClosestPointOnBounds(destination), Time.smoothDeltaTime);
            FollowTarget.transform.position = lerpedPosition;
        }
    }

    private IEnumerator Coroutine_RotateCamera(bool clockwise)
    {
        _rotating = true;

        float originAngle = CM_Main.transform.eulerAngles.y;
        float step = EnvironmentCameraSettings.RotationStep;

        if (!clockwise) step *= -1;

        float targetAngle = originAngle + step;

        float currentTime = 0;

        while (currentTime < EnvironmentCameraSettings.RotationDuration)
        {
            float currentAngle = Mathf.LerpAngle(originAngle, targetAngle, currentTime / EnvironmentCameraSettings.RotationDuration);
            CM_Main.transform.eulerAngles = new Vector3(45, currentAngle, 0);

            currentTime += Time.smoothDeltaTime;

            yield return new WaitForFixedUpdate();
        }

        CM_Main.transform.eulerAngles = new Vector3(45, targetAngle, 0);

        _rotating = false;
    }

    public void SwitchToMainCam()
    {
        CM_Overview.Priority = 0;
        CM_Main.Priority = 10;
    }

    public void SwitchToOverviewCam()
    {
        CM_Main.Priority = 0;
        CM_Overview.Priority = 10;
    }

}
