using System.Collections;
using System.Collections.Generic;
using Constants;
using Cinemachine;
using UnityEngine;

public class CharacterCameraManager : MonoBehaviour
{
    private static CharacterCameraManager _instance;

    public static CharacterCameraManager Instance { get { return _instance; } }

    private Dictionary<CharacterComponent, CinemachineVirtualCamera> Cameras;

    private int _disabledPriority = 0;
    private int _enabledPriority = 20;

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
        Cameras = new Dictionary<CharacterComponent, CinemachineVirtualCamera>();
    }

    private void DisableCameras()
    {
        foreach(CinemachineVirtualCamera cam in Cameras.Values)
        {
            cam.Priority = _disabledPriority;
        }
    }

    public void RegisterCharacterCamera(CharacterComponent character)
    {
        GameObject newCamera = PrefabFactory.CreatePrefab(RegistryID.CM_Character, this.transform);

        newCamera.GetComponent<CinemachineVirtualCamera>().Follow = character.GetComponentInChildren<NavigatorComponent>().transform;

        if(character.gameObject.GetComponent<NPCComponent>() && Cameras.ContainsKey(character) == false)
        {
            newCamera.GetComponent<CinemachineVirtualCamera>().Priority = _disabledPriority;
            Cameras.Add(character, newCamera.GetComponent<CinemachineVirtualCamera>());
        }
    }

    public void UnRegisterCharacterCamera(CharacterComponent character)
    {
        if (character.gameObject.GetComponent<NPCComponent>())
        {
            GameObject camera = null;

            if (Cameras[character] != null)
            {
                camera = Cameras[character].gameObject;
            }

            Cameras.Remove(character);

            Destroy(camera);
        }
    }

    public void SelectCharacterCamera(CharacterComponent npc)
    {
        CinemachineVirtualCamera camera = Cameras[npc];

        DisableCameras();

        DebugManager.Instance.Print(DebugManager.Log.LogCameraManager, "Switching to camera " + camera);

        camera.Priority = _enabledPriority;  
    }

    public void UnselectCharacterCamera()
    {
        DisableCameras();

        DebugManager.Instance.Print(DebugManager.Log.LogCameraManager, "Switching to default camera");
    }
}
