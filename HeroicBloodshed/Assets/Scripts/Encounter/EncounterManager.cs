using System;
using System.Collections;
using System.Collections.Generic;

using Cinemachine;
using UnityEngine;
using static Constants;

public class EncounterManager : MonoBehaviour
{
    [Header("Setup Data")]
    [SerializeField] private EncounterSetupData _setupData; //setup data

    [Header("Prefabs")]
    [SerializeField] private GameObject Prefab_EncounterCameraRig;
    [SerializeField] private GameObject Prefab_EncounterCanvas;

    private static EncounterManager _instance;

    public static EncounterManager Instance { get { return _instance; } }

    private Encounter _encounter;
    private EncounterCanvas _canvas;
    private EncounterCameraRig _cameraRig;

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
        LoadEncounter();
    }

    public void LoadEncounter()
    {
        StartCoroutine(Coroutine_LoadEncounter());
    }

    private void EncounterStateCallback(Encounter encounter)
    {
        StartCoroutine(Coroutine_EncounterStateCallback(encounter));
    }

    private IEnumerator Coroutine_LoadEncounter()
    {
        //create a canvas 
        GameObject canvasObject = Instantiate(Prefab_EncounterCanvas, null);
        yield return new WaitWhile(() => canvasObject.GetComponent<EncounterCanvas>() == null);
        _canvas = canvasObject.GetComponent<EncounterCanvas>();

        //create a camera rig
        GameObject cameraRigObject = Instantiate(Prefab_EncounterCameraRig, null);
        yield return new WaitWhile(() => cameraRigObject.GetComponent<EncounterCameraRig>() == null);
        _cameraRig = cameraRigObject.GetComponent<EncounterCameraRig>();
        _cameraRig.Setup(_setupData.CameraFollowTarget);

        //generate encounter and attach to handler
        _encounter = _setupData.gameObject.AddComponent<Encounter>();
        yield return new WaitWhile(() => _setupData.gameObject.GetComponent<Encounter>() == null);
        _encounter.SetSetupData(_setupData);

        _encounter.OnStateChanged += this.EncounterStateCallback;

        yield return _canvas.HandleInit();
        yield return _encounter.HandleInit();

        _encounter.TransitionState();

        yield return null;
    }

    public void SelectAbility(AbilityID abilityID)
    {
        Debug.Log("ability selected " + abilityID);

        _encounter.TransitionState();
    }

    private IEnumerator Coroutine_WaitForPlayerInput()
    {
        Debug.Log("Waiting for player input");

        yield return null;
    }

    private IEnumerator Coroutine_EncounterStateCallback(Encounter encounter)
    {
        EncounterState state = encounter.GetState();

        _canvas.UpdateCanvas(encounter);

        switch (state)
        {
            case EncounterState.BUILD_QUEUES:
            {
                _cameraRig.GoToMainCamera();
                yield return new WaitForSeconds(1.0f);
                break;
            }
            case EncounterState.PERFORM_ACTION:
            {
                yield return new WaitForSeconds(1f);
                break;
            }
            case EncounterState.SELECT_CURRENT_CHARACTER:
            {
                CharacterComponent character = encounter.GetCurrentCharacter();
                _cameraRig.GoToCharacter(character);
                character.CreateDecal();
                break;
            }
            case EncounterState.WAIT_FOR_PLAYER_INPUT:
            {
                yield return Coroutine_WaitForPlayerInput();
                yield break;
            }
            case EncounterState.CHOOSE_AI_ACTION:
            {
                yield return new WaitForSeconds(1.0f);
                break;
            }
            case EncounterState.DESELECT_CURRENT_CHARACTER:
            {
                CharacterComponent character = encounter.GetCurrentCharacter();
                character.DestroyDecal();
                break;
            }
            case EncounterState.DONE:
            {
                _cameraRig.GoToMainCamera();
                break;
            }
            default:
            {
                break;
            }
        }

        yield return new WaitForSeconds(0.2f);

        encounter.TransitionState();

        if(state == EncounterState.DONE)
        {
            CleanUpCurrentEncounter();
        }

        yield return null;
    }

    private void CleanUpCurrentEncounter()
    {
        if(_encounter != null)
        {
            _encounter.OnStateChanged -= this.EncounterStateCallback;

            Destroy(_encounter.gameObject);
            _encounter = null;
        }
    }

    public void FollowCharacter(CharacterComponent characterComponent)
    {
        if (_encounter.GetState() == EncounterState.WAIT_FOR_PLAYER_INPUT)
        {
            _cameraRig.GoToCharacter(characterComponent);
        }
    }

    public void UnfollowCharacter()
    {
        if (_encounter.GetState() == EncounterState.WAIT_FOR_PLAYER_INPUT)
        {
            FollowCharacter(_encounter.GetCurrentCharacter());
        }
    }

    public IEnumerator SpawnCharacters(Encounter encounter)
    {
        foreach (CharacterComponent characterComponent in encounter.GetAllCharacters())
        {
            yield return characterComponent.SpawnCharacter();
        }
    }

    public void OnCharacterSpawned(CharacterComponent characterComponent)
    {
        if (_cameraRig != null)
        {
            _cameraRig.RegisterCharacterCamera(characterComponent);
        }
    }

    public IEnumerator DespawnCharacters(Encounter encounter)
    {
        foreach (CharacterComponent characterComponent in encounter.GetAllCharacters())
        {
            yield return characterComponent.PerformCleanup();
        }
    }

    public void OnCharacterDespawned(CharacterComponent characterComponent)
    {
        if (_cameraRig != null)
        {
            _cameraRig.UnregisterCharacterCamera(characterComponent);
        }
    }
}
