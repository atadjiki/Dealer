using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnvironmentCameraSettings
{
    [Header("Angle")]
    public float Angle = 45;

    [Header("Zoom")]
    public float MaxDistance = 30;
    public float DefaultDistance = 20;
    public float MinDistance = 10;

    public float ZoomSpeed = 75;

    [Header("Rotation")]
    public float DefaultRotation = 0; 
    public float RotationStep = 90;
    public float RotationDuration = 0.35f;
    
    [Header("Movement")]
    public float CameraSpeed = 15f;
    public float ScreenEdgeDelta = 1;
    public float BoundaryWidth = 32;

}

public class EnvironmentCameraRig : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private EnvironmentCameraSettings CameraSettings;

    private CinemachineVirtualCamera _camera;
    private CinemachineFramingTransposer _framingTransposer;
    private EnvironmentCameraTarget _followTarget;

    private BoxCollider _collider;

    private bool _rotating = false;

    private void Awake()
    {
        _collider = this.gameObject.AddComponent<BoxCollider>();
        _collider.isTrigger = true;
        _collider.size = new Vector3(CameraSettings.BoundaryWidth, 0, CameraSettings.BoundaryWidth);

        _followTarget = GetComponentInChildren<EnvironmentCameraTarget>();

        SetupCamera();
    }

    private void SetupCamera()
    {
        GameObject CameraObject = new GameObject("CM_Environment");
        CameraObject.transform.parent = this.transform;

        _camera = CameraObject.AddComponent<CinemachineVirtualCamera>();
        _camera.transform.eulerAngles = new Vector3(CameraSettings.Angle, CameraSettings.DefaultRotation, 0);
        _camera.Follow = _followTarget.transform;

        _framingTransposer =_camera.AddCinemachineComponent<CinemachineFramingTransposer>();
        _framingTransposer.m_CameraDistance = CameraSettings.DefaultDistance;
        _framingTransposer.m_MinimumDistance = CameraSettings.MinDistance;
        _framingTransposer.m_MaximumDistance = CameraSettings.MaxDistance;
    }

    private void Update()
    {
        Update_Zoom();
        Update_Rotation();
        Update_Movement_Keys();
        Update_Movement_Mouse();
    }

    private void Update_Zoom()
    {
        Vector2 mouseScrollDelta = Input.mouseScrollDelta;

        if (mouseScrollDelta.magnitude > 0)
        {
            float currentDistance =_framingTransposer.m_CameraDistance;

            float targetDistance = CameraSettings.MinDistance;

            if(mouseScrollDelta.y < 0) { targetDistance = CameraSettings.MaxDistance; }

            currentDistance = Mathf.Lerp(currentDistance, targetDistance, Time.smoothDeltaTime * CameraSettings.ZoomSpeed);

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

        if (Input.mousePosition.x >= Screen.width - CameraSettings.ScreenEdgeDelta)
        {
            direction.x = 1;
        }
        else if (Input.mousePosition.x <= 0 + CameraSettings.ScreenEdgeDelta)
        {
            direction.x = -1;
        }

        if (Input.mousePosition.y >= Screen.height - CameraSettings.ScreenEdgeDelta)
        {
            direction.z = 1;
        }
        else if (Input.mousePosition.y <= 0 + CameraSettings.ScreenEdgeDelta)
        {
            direction.z = -1;
        }

        Update_Movement(direction);
    }

    private void Update_Movement(Vector3 direction)
    {
        direction *= CameraSettings.CameraSpeed;

        //camera forward and right vectors:
        Vector3 forward = Camera.main.transform.forward;
        forward.y = 0;

        Vector3 right = Camera.main.transform.right;

        direction = forward * direction.z + right * direction.x;

        Vector3 origin = _followTarget.transform.position;
        Vector3 destination = _followTarget.transform.position + direction;
        Vector3 lerpedPosition = Vector3.Lerp(origin, destination, Time.smoothDeltaTime);

        if (_collider.bounds.Contains(lerpedPosition))
        {
            _followTarget.transform.position = lerpedPosition;
        }
        else
        {
            lerpedPosition = Vector3.Lerp(origin, _collider.ClosestPointOnBounds(destination), Time.smoothDeltaTime);
            _followTarget.transform.position = lerpedPosition;
        }
    }

    private IEnumerator Coroutine_RotateCamera(bool clockwise)
    {
        _rotating = true;

        float originAngle = _camera.transform.eulerAngles.y;
        float step = CameraSettings.RotationStep;

        if (!clockwise) step *= -1;

        float targetAngle = originAngle + step;

        float currentTime = 0;

        while (currentTime < CameraSettings.RotationDuration)
        {
            float currentAngle = Mathf.LerpAngle(originAngle, targetAngle, currentTime / CameraSettings.RotationDuration);
            _camera.transform.eulerAngles = new Vector3(CameraSettings.Angle, currentAngle, 0);

            currentTime += Time.smoothDeltaTime;

            yield return new WaitForFixedUpdate();
        }

        _camera.transform.eulerAngles = new Vector3(CameraSettings.Angle, targetAngle, 0);

        _rotating = false;
    }

}
