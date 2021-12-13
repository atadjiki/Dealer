using System.Collections;
using System.Collections.Generic;
using Constants;
using Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public enum CameraDirection { Front, Backward, Left, Right };
    public enum CameraZoom { Close, Normal, Far };

    [System.Serializable]
    public struct OverheadCameraData
    {
        public CinemachineVirtualCamera cam;
        public CameraDirection direction;
    }

    private static CameraManager _instance;

    public static CameraManager Instance { get { return _instance; } }

    private Dictionary<CharacterComponent, CinemachineVirtualCamera> CharacterCameras;
    [SerializeField] private OverheadCameraData[] OverheadCameras;

    private Vector3 Vec_Front = new Vector3(0, 1, -1);
    private Vector3 Vec_Back = new Vector3(0, 1, 1);
    private Vector3 Vec_Left = new Vector3(-1, 1, 0);
    private Vector3 Vec_Right = new Vector3(1, 1, 0);

    [SerializeField] private float Zoom_Close = 5.0f;
    [SerializeField] private float Zoom_Normal = 15;
    [SerializeField] private float Zoom_Far = 25;

    private int _disabledPriority = 0;
    private int _enabledPriority = 20;

    [SerializeField] private CameraDirection DefaultOverheadCamera = CameraDirection.Backward;
    [SerializeField] private CameraZoom DefaultOverheadZoom = CameraZoom.Normal;

    private CameraDirection _currentDirection;
    private CameraZoom _currentZoom;

    private CinemachineVirtualCamera currentOverheadCam;

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
        CharacterCameras = new Dictionary<CharacterComponent, CinemachineVirtualCamera>();

        _currentDirection = DefaultOverheadCamera;
        _currentZoom = DefaultOverheadZoom;

        SwitchToOverheadCamera(_currentDirection);
        SetOverheadZoom(_currentZoom);
    }

    private void DisableCameras()
    {
        foreach(CinemachineVirtualCamera cam in CharacterCameras.Values)
        {
            cam.Priority = _disabledPriority;
        }

        foreach(OverheadCameraData data in OverheadCameras)
        {
            data.cam.Priority = _disabledPriority;
        }
    }

    private void SwitchToOverheadCamera(CameraDirection direction)
    {
        DisableCameras();

        foreach(OverheadCameraData data in OverheadCameras)
        {
            if(data.direction == direction)
            {
                data.cam.Priority = _enabledPriority;
                currentOverheadCam = data.cam;
                return;
            }
        }
    }

    private void SetOverheadZoom(CameraZoom zoom)
    {
        foreach(OverheadCameraData data in OverheadCameras)
        {
            Vector3 offset = GetVectorByDirection(data.direction) * GetZoomLevel(zoom);

            data.cam.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset = offset;
        }
    }

    public void RegisterCharacterCamera(CharacterComponent character)
    {
        GameObject newCamera = PrefabFactory.Instance.CreatePrefab(RegistryID.CM_Character, this.transform);

        newCamera.GetComponent<CinemachineVirtualCamera>().Follow = character.GetComponentInChildren<NavigatorComponent>().transform;

        if(character.gameObject.GetComponent<NPCComponent>() && CharacterCameras.ContainsKey(character) == false)
        {
            newCamera.GetComponent<CinemachineVirtualCamera>().Priority = _disabledPriority;
            CharacterCameras.Add(character, newCamera.GetComponent<CinemachineVirtualCamera>());
        }
    }

    public void UnRegisterCharacterCamera(CharacterComponent character)
    {
        if (character.gameObject.GetComponent<NPCComponent>())
        {
            GameObject camera = null;

            if (CharacterCameras[character] != null)
            {
                camera = CharacterCameras[character].gameObject;
            }

            CharacterCameras.Remove(character);

            Destroy(camera);
        }
    }

    public void SelectCharacterCamera(CharacterComponent npc)
    {
        CinemachineVirtualCamera camera = CharacterCameras[npc];

        if(camera != null)
        {
            foreach(CinemachineVirtualCamera toSuppress in CharacterCameras.Values)
            {
                toSuppress.Priority = _disabledPriority;
            }
        }

        if (DebugManager.Instance.LogCameraManager) Debug.Log("Switching to camera " + camera);

        camera.Priority = _enabledPriority;  
    }

    public void UnselectCharacterCamera()
    {
        foreach(CinemachineVirtualCamera toSuppress in CharacterCameras.Values)
        {
            toSuppress.Priority = _disabledPriority;
        }

        if (DebugManager.Instance.LogCameraManager) Debug.Log("Switching to default camera");
    }

    private float GetZoomLevel(CameraZoom level)
    {
        switch (level)
        {
            case CameraZoom.Close:
                return Zoom_Close;
            case CameraZoom.Normal:
                return Zoom_Normal;
            case CameraZoom.Far:
                return Zoom_Far;
        }

        return 0;
    }

    public Vector3 GetVectorByDirection(InputManager.InputDirection direction)
    {
        switch (direction)
        {
            case InputManager.InputDirection.Up:
                return currentOverheadCam.transform.forward;
            case InputManager.InputDirection.Down:
                return currentOverheadCam.transform.forward * -1;
            case InputManager.InputDirection.Left:
                return currentOverheadCam.transform.right*-1;
            case InputManager.InputDirection.Right:
                return currentOverheadCam.transform.right;
            default:
                return Vector3.zero;
        }
    }

    private Vector3 GetVectorByDirection(CameraDirection direction)
    {
        switch (direction)
        {
            case CameraDirection.Backward:
                return Vec_Back;
            case CameraDirection.Front:
                return Vec_Front;
            case CameraDirection.Left:
                return Vec_Left;
            case CameraDirection.Right:
                return Vec_Right;
        }

        return Vector3.zero;
    }

    public void RotateClockwise()
    {
        _currentDirection = GetCWDirection(_currentDirection);
        SwitchToOverheadCamera(_currentDirection);
    }

    public void RotateCounterClockwise()
    {
        _currentDirection = GetCCWDirection(_currentDirection);
        SwitchToOverheadCamera(_currentDirection);
    }

    public void ZoomOut()
    {
        _currentZoom = GetPreviousZoom(_currentZoom);
        SetOverheadZoom(_currentZoom);
    }

    public void ZoomIn()
    {
        _currentZoom = GetNextZoom(_currentZoom);
        SetOverheadZoom(_currentZoom);
    }

    private CameraZoom GetNextZoom(CameraZoom zoom)
    {
        switch(zoom)
        {
            case CameraZoom.Close:
                return CameraZoom.Close;
            case CameraZoom.Normal:
                return CameraZoom.Close;
            case CameraZoom.Far:
                return CameraZoom.Normal;
        }

        return zoom;
    }

    private CameraZoom GetPreviousZoom(CameraZoom zoom)
    {
        switch (zoom)
        {
            case CameraZoom.Close:
                return CameraZoom.Normal;
            case CameraZoom.Normal:
                return CameraZoom.Far;
            case CameraZoom.Far:
                return CameraZoom.Far;
        }

        return zoom;
    }

    private CameraDirection GetCWDirection(CameraDirection dir)
    {
        switch(dir)
        {
            case CameraDirection.Front:
                return CameraDirection.Right;
            case CameraDirection.Right:
                return CameraDirection.Backward;
            case CameraDirection.Backward:
                return CameraDirection.Left;
            case CameraDirection.Left:
                return CameraDirection.Front;
        }

        return dir;
    }

    private CameraDirection GetCCWDirection(CameraDirection dir)
    {
        switch (dir)
        {
            case CameraDirection.Front:
                return CameraDirection.Left;
            case CameraDirection.Left:
                return CameraDirection.Backward;
            case CameraDirection.Backward:
                return CameraDirection.Right;
            case CameraDirection.Right:
                return CameraDirection.Front;
        }

        return dir;
    }

    public Vector3 GetCurrentDirection()
    {
        return GetVectorByDirection(_currentDirection);
    }
}
