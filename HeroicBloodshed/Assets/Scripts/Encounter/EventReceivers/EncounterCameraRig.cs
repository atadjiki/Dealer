using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using static Constants;

//DEPRECATED
[Obsolete("Use EnvironmentCameraRig instead")]
public class EncounterCameraRig : EncounterEventReceiver
{
    [Header("Prefabs")]
    [SerializeField] private GameObject Prefab_DefaultCamera;
    [SerializeField] private GameObject Prefab_CharacterCamera;

    private CinemachineVirtualCamera CM_Main;
    private Dictionary<CharacterComponent, CinemachineVirtualCamera> _cameraMap;

    private static int _priorityMain = 5;
    private static int _priorityInactive = 0;
    private static int _priorityActive = 10;

    private static int _zoomDefault = 12;
    private static int _zoomCharacter = 11;

    public override IEnumerator Coroutine_Init(EncounterModel model)
    {
        _cameraMap = new Dictionary<CharacterComponent, CinemachineVirtualCamera>();

        GameObject defaultCameraObject = Instantiate(Prefab_DefaultCamera, this.transform);
        CM_Main = defaultCameraObject.GetComponent<CinemachineVirtualCamera>();

        //CM_Main.Follow = model.GetCameraFollow();
        CM_Main.Priority = _priorityMain;

        CinemachineFramingTransposer framingTransposer = CM_Main.AddCinemachineComponent<CinemachineFramingTransposer>();
        framingTransposer.m_CameraDistance = _zoomDefault;

        foreach(CharacterComponent character in model.GetAllCharacters())
        {
            RegisterCharacterCamera(character);
        }

        yield return null;
    }

    public override IEnumerator Coroutine_StateUpdate(EncounterState stateID, EncounterModel model)
    {
        switch (stateID)
        {
            case EncounterState.BUILD_QUEUES:
                {
                    GoToMainCamera();
                    break;
                }
            case EncounterState.TEAM_UPDATED:
                {
                    GoToMainCamera();
                    break;
                }
            case EncounterState.CHOOSE_TARGET:
                {
                    GoToMainCamera();
                    break;
                }
            case EncounterState.SELECT_CURRENT_CHARACTER:
                {
                    CharacterComponent character = model.GetCurrentCharacter();
                    if (character.IsAlive())
                    {
                        GoToCharacter(character);
                    }

                    break;
                }
            case EncounterState.DONE:
                {
                    GoToMainCamera();
                    break;
                }
            default:
                {
                    break;
                }
        }

        yield return null;
    }

    private void RegisterCharacterCamera(CharacterComponent characterComponent)
    {
        if(_cameraMap != null && !_cameraMap.ContainsKey(characterComponent))
        {
            GameObject characterCameraObject = Instantiate(Prefab_CharacterCamera, this.transform);

            CinemachineVirtualCamera CM_Character = characterCameraObject.GetComponent<CinemachineVirtualCamera>();

            CM_Character.Follow = characterComponent.transform;
            CM_Character.Priority = _priorityInactive;

            CinemachineFramingTransposer framingTransposer = CM_Character.AddCinemachineComponent<CinemachineFramingTransposer>();
            framingTransposer.m_CameraDistance = _zoomCharacter;

            _cameraMap.Add(characterComponent, CM_Character);
        }
    }

    private void UnregisterCharacterCamera(CharacterComponent characterComponent)
    {
        if (_cameraMap != null && _cameraMap.ContainsKey(characterComponent))
        {
            CinemachineVirtualCamera CM_Character = _cameraMap[characterComponent];

            Destroy(CM_Character.gameObject);
        }
    }

    private CinemachineVirtualCamera GetCharacterCamera(CharacterComponent characterComponent)
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


    //public interface
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
