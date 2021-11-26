using System.Collections;
using System.Collections.Generic;
using Constants;
using Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private static CameraManager _instance;

    public static CameraManager Instance { get { return _instance; } }

    private Camera _mainCamera;

    private Dictionary<CharacterComponent, CinemachineVirtualCamera> CharacterCameras;

    private int _npcPriority = 0;
    private int _defaultPriority = 15;
    private int _selectedPriority = 20;

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

        _mainCamera = GetComponentInChildren<Camera>();
    }

    public void RegisterCharacterCamera(CharacterComponent character)
    {
        GameObject newCamera = PrefabFactory.Instance.CreatePrefab(RegistryID.CM_Character, this.transform);

        newCamera.GetComponent<CinemachineVirtualCamera>().Follow = character.GetComponentInChildren<NavigatorComponent>().transform;

        if(character.gameObject.GetComponent<NPCComponent>() && CharacterCameras.ContainsKey(character) == false)
        {
            newCamera.GetComponent<CinemachineVirtualCamera>().Priority = _npcPriority;
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

    public Camera GetMainCamera()
    {
        return _mainCamera;
    }

    public void SelectCharacterCamera(CharacterComponent npc)
    {
        CinemachineVirtualCamera camera = CharacterCameras[npc];

        if(camera != null)
        {
            foreach(CinemachineVirtualCamera toSuppress in CharacterCameras.Values)
            {
                toSuppress.Priority = _npcPriority;
            }
        }

        if (DebugManager.Instance.LogCameraManager) Debug.Log("Switching to camera " + camera);

        camera.Priority = _selectedPriority;  
    }

    public void UnselectCharacterCamera()
    {
        foreach(CinemachineVirtualCamera toSuppress in CharacterCameras.Values)
        {
            toSuppress.Priority = _npcPriority;
        }

        if (DebugManager.Instance.LogCameraManager) Debug.Log("Switching to default camera");
    }
}
