using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class EncounterCameraRig : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject Prefab_DefaultCamera;
    [SerializeField] private GameObject Prefab_CharacterCamera;

    private CinemachineVirtualCamera CM_Main;
    private Dictionary<CharacterComponent, CinemachineVirtualCamera> _cameraMap;

    private static int _priorityMain = 5;
    private static int _priorityInactive = 0;
    private static int _priorityActive = 10;

    private void Awake()
    {
        CM_Main = null;
        _cameraMap = new Dictionary<CharacterComponent, CinemachineVirtualCamera>();
        
        CM_Main.Priority = _priorityMain;
    }

    public void Setup(Transform defaultFollowTarget)
    {
        GameObject defaultCameraObject = Instantiate(Prefab_DefaultCamera, this.transform);
        CM_Main = defaultCameraObject.GetComponent<CinemachineVirtualCamera>();

        CM_Main.Follow = defaultFollowTarget;
        CM_Main.Priority = _priorityMain;
        
        CinemachineFramingTransposer framingTransposer = CM_Main.AddCinemachineComponent<CinemachineFramingTransposer>();
        framingTransposer.m_CameraDistance = 12;
    }

    public void RegisterCharacterCamera(CharacterComponent characterComponent)
    {
        if(_cameraMap != null && !_cameraMap.ContainsKey(characterComponent))
        {
            GameObject characterCameraObject = Instantiate(Prefab_CharacterCamera, this.transform);

            CinemachineVirtualCamera CM_Character = characterCameraObject.GetComponent<CinemachineVirtualCamera>();

            CM_Character.Follow = characterComponent.transform;
            CM_Character.Priority = _priorityInactive;

            CinemachineFramingTransposer framingTransposer = CM_Character.AddCinemachineComponent<CinemachineFramingTransposer>();
            framingTransposer.m_CameraDistance = 6;

            _cameraMap.Add(characterComponent, CM_Character);
        }
    }

    public void UnregisterCharacterCamera(CharacterComponent characterComponent)
    {
        if (_cameraMap != null && _cameraMap.ContainsKey(characterComponent))
        {
            CinemachineVirtualCamera CM_Character = _cameraMap[characterComponent];

            Destroy(CM_Character.gameObject);
        }
    }

    public CinemachineVirtualCamera GetCharacterCamera(CharacterComponent characterComponent)
    {
        if (_cameraMap != null && _cameraMap.ContainsKey(characterComponent))
        {
            return _cameraMap[characterComponent];
        }

        return null;
    }

    private void DisableCharacterCameras()
    {
        foreach(CinemachineVirtualCamera CM_Character in _cameraMap.Values)
        {
            CM_Character.Priority = _priorityInactive;
        }
    }

    public void GoToCharacter(CharacterComponent characterComponent)
    {
        CinemachineVirtualCamera CM_Character = GetCharacterCamera(characterComponent);

        if (CM_Character != null)
        {
            DisableCharacterCameras();
            CM_Character.Priority = _priorityActive;
        }
    }

    public void GoToMainCamera()
    {
        DisableCharacterCameras();
        CM_Main.Priority = _priorityMain;
    }
}
