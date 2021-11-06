using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private static CameraManager _instance;

    public static CameraManager Instance { get { return _instance; } }

    private Dictionary<CharacterComponent, CinemachineVirtualCamera> CharacterCameras;
    private CinemachineVirtualCamera PlayerCamera;
    private GameObject CharacterCameraPrefab;

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
        CharacterCameraPrefab = Resources.Load<GameObject>("CM_Character");
        CharacterCameras = new Dictionary<CharacterComponent, CinemachineVirtualCamera>();
    }

    public void RegisterCharacterCamera(CharacterComponent character)
    {
        GameObject newCamera = Instantiate<GameObject>(CharacterCameraPrefab, this.transform);
        newCamera.transform.parent = this.transform;

        newCamera.GetComponent<CinemachineVirtualCamera>().Follow = character.GetComponentInChildren<Navigator>().transform;

        if(character.gameObject.GetComponent<PlayerController>())
        {
            newCamera.GetComponent<CinemachineVirtualCamera>().Priority = _playerPriority;
            PlayerCamera = newCamera.GetComponent<CinemachineVirtualCamera>();
        }
        else if(character.gameObject.GetComponent<NPCController>())
        {
            newCamera.GetComponent<CinemachineVirtualCamera>().Priority = _npcPriority;
            CharacterCameras.Add(character, newCamera.GetComponent<CinemachineVirtualCamera>());
        }
    }

    public void UnRegisterCharacterCamera(CharacterComponent character)
    {
        if(character.gameObject.GetComponent<NPCController>())
        {
            GameObject camera = null;

            if (CharacterCameras[character] != null)
            {
                camera = CharacterCameras[character].gameObject;
            }

            CharacterCameras.Remove(character);

            Destroy(camera);
        }
        else if(character.gameObject.GetComponent<PlayerController>())
        {
            if(PlayerCamera != null)
                Destroy(PlayerCamera.gameObject);
        }
        
    }
}
