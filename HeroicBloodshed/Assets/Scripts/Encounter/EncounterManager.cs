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

    [Header("Camera")]
    [SerializeField] private CinemachineVirtualCamera _virtualCamera;

    [Header("Canvas")]
    [SerializeField] private GameObject Prefab_EncounterCanvas;

    private void Awake()
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
        EncounterCanvas canvas = canvasObject.GetComponent<EncounterCanvas>();
        canvas.OnAbilitySelected += OnAbilitySelected;

        //generate encounter and attach to handler
        Encounter encounter = _setupData.gameObject.AddComponent<Encounter>();
        yield return new WaitWhile(() => encounter.GetComponent<Encounter>() == null);
        encounter.SetSetupData(_setupData);

        encounter.OnStateChanged += canvas.EncounterStateCallback;
        encounter.OnStateChanged += this.EncounterStateCallback;

        yield return canvas.HandleInit();
        yield return encounter.HandleInit();

        encounter.TransitionState();

        yield return null;
    }

    private void OnAbilitySelected(AbilityID abilityID)
    {
        Debug.Log("ability selected " + abilityID);


    }

    private IEnumerator Coroutine_WaitForPlayerInput()
    {
        Debug.Log("Waiting for player input");

        yield return null;
    }

    private IEnumerator Coroutine_EncounterStateCallback(Encounter encounter)
    {
        EncounterState state = encounter.GetState();

        switch (state)
        {
            case EncounterState.BUILD_QUEUES:
            {
                SetCameraFollow(encounter.GetCameraFollow());
                break;
            }
            case EncounterState.PERFORM_ACTION:
            {
                yield return new WaitForSeconds(1f);
                break;
            }
            case EncounterState.SELECT_CURRENT_CHARACTER:
            {
                SetCameraFollow(encounter.GetCurrentCharacter().transform);
                CharacterComponent character = encounter.GetCurrentCharacter();
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
                ResetCameraFollow();
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
            CleanUpEncounter(encounter);
        }

        yield return null;
    }

    private void CleanUpEncounter(Encounter encounter)
    {
        encounter.OnStateChanged -= this.EncounterStateCallback;

        Destroy(encounter.gameObject);
        encounter = null;
    }

    public void FollowCharacter(CharacterComponent characterComponent)
    {
        Transform characterTransform = characterComponent.transform;
        SetCameraFollow(characterTransform);
    }


    private void ToggleCamera(bool flag) { _virtualCamera.enabled = flag; }
    private void SetCameraFollow(Transform target) { _virtualCamera.Follow = target; }
    public void ResetCameraFollow() { SetCameraFollow(this.transform); }
}
