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
    private CinemachineVirtualCamera PlayerCamera;
   

    private int _npcPriority = 10;
    private int _playerPriority = 15;
    //private int _defaultPriority = 5;

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
       // CharacterCameraPrefab = Resources.Load<GameObject>(ResourcePaths.CM_Character);
        CharacterCameras = new Dictionary<CharacterComponent, CinemachineVirtualCamera>();

        _mainCamera = GetComponentInChildren<Camera>();
    }

    public void RegisterCharacterCamera(CharacterComponent character)
    {
        GameObject newCamera = PrefabFactory.Instance.CreatePrefab(Prefab.CM_Character, this.transform);
       // newCamera.transform.parent = this.transform;

        newCamera.GetComponent<CinemachineVirtualCamera>().Follow = character.GetComponentInChildren<NavigatorComponent>().transform;

        if(character.gameObject.GetComponent<PlayerComponent>())
        {
            newCamera.GetComponent<CinemachineVirtualCamera>().Priority = _playerPriority;
            PlayerCamera = newCamera.GetComponent<CinemachineVirtualCamera>();
        }
        else if(character.gameObject.GetComponent<NPCComponent>())
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
        else if (character.gameObject.GetComponent<PlayerComponent>())
        {
            if (PlayerCamera != null)
                Destroy(PlayerCamera.gameObject);
        }
    }

    public Camera GetMainCamera()
    {
        return _mainCamera;
    }
}
