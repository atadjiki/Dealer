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

    private void Awake()
    {
        LoadEncounter();
    }

    public void LoadEncounter()
    {
        Encounter encounter = _setupData.gameObject.AddComponent<Encounter>();
        encounter.OnStateChanged += EncounterStateCallback;
        encounter.SetSetupData(_setupData);
        encounter.TransitionState();
    }

    private void EncounterStateCallback(Encounter encounter, EncounterState state)
    {
        switch (state)
        {
            case EncounterState.SETUP:
            case EncounterState.BUILD_QUEUES:
            case EncounterState.CHECK_CONDITIONS:
            case EncounterState.PROCESS_TURN:
            case EncounterState.SELECT_CURRENT_CHARACTER:
            case EncounterState.WAIT_FOR_PLAYER_INPUT:
            case EncounterState.CHOOSE_AI_ACTION:
            case EncounterState.PERFORM_ACTION:
            case EncounterState.DESELECT_CURRENT_CHARACTER:
            case EncounterState.UPDATE:
                encounter.TransitionState();
                break;
            case EncounterState.DONE:
                CleanUpEncounter(encounter);
                break;
            default:
                Debug.Log("No state transition available!");
                break;
        }
    }

    private void CleanUpEncounter(Encounter encounter)
    {
        encounter.OnStateChanged -= EncounterStateCallback;
        //Destroy(encounter.gameObject);
        encounter = null;
    }

    private void ToggleCamera(bool flag) { _virtualCamera.enabled = flag; }
    private void SetCameraFollow(Transform target) { _virtualCamera.Follow = target; }
    private void ResetCameraFollow() { SetCameraFollow(null); }
}
