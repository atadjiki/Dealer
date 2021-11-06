using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private static CameraManager _instance;

    public static CameraManager Instance { get { return _instance; } }

    private Dictionary<CharacterComponent, CinemachineVirtualCamera> CharacterCameras;
    private GameObject CharacterCameraPrefab;

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
        newCamera.GetComponent<CinemachineVirtualCamera>().Follow = character.gameObject.transform;
        CharacterCameras.Add(character, newCamera.GetComponent<CinemachineVirtualCamera>());
    }

    public void UnRegisterCharacterCamera(CharacterComponent character)
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
