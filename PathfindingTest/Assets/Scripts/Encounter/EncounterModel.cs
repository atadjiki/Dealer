using System.Collections;
using UnityEditor;
using UnityEngine;
using static Constants;

public class EncounterModel : MonoBehaviour
{
    public static void Init()
    {
        //make a fresh state data for the encounter
        EncounterStateData stateData = EncounterStateData.Build();

        stateData.SetPendingState(EncounterState.INIT);
        Transition(stateData);
    }

    public static void Transition(EncounterStateData stateData)
    {
        EncounterState pendingState = stateData.CurrentState;

        Debug.Log("Model - Handling State " + pendingState);

        switch (pendingState)
        {
            case EncounterState.INIT:
                stateData.SetPendingState(EncounterState.SETUP_START);
                break;
            case EncounterState.SETUP_START:
                stateData.SetPendingState(EncounterState.SETUP_COMPLETE);
                break;
            case EncounterState.SETUP_COMPLETE:
                stateData.SetPendingState(EncounterState.BUILD_QUEUES);
                break;
            case EncounterState.BUILD_QUEUES:
                stateData.BuildTimeline();
                stateData.SetPendingState(EncounterState.CHECK_CONDITIONS);
                break;
            case EncounterState.CHECK_CONDITIONS:
                //if any queues are empty, then we are done with the encounter
                if (stateData.AreAnyTeamsDead())
                {
                    //call delegate
                    stateData.SetPendingState(EncounterState.DONE);
                    return;
                }
                stateData.SetPendingState(EncounterState.SELECT_CURRENT_CHARACTER);
                break;
            case EncounterState.SELECT_CURRENT_CHARACTER:
                if (stateData.IsCurrentCharacterAlive())
                {
                    stateData.SetPendingState(EncounterState.CHOOSE_ACTION);
                }
                else
                {
                    stateData.SetPendingState(EncounterState.DESELECT_CURRENT_CHARACTER);
                }
                break;
            case EncounterState.TEAM_UPDATED:
                stateData.SetPendingState(EncounterState.SELECT_CURRENT_CHARACTER);
                break;
            case EncounterState.CHOOSE_ACTION:
                //check if we need to choose a target for this action
                AbilityID abilityID = stateData.GetActiveAbility();

                switch (GetTargetType(abilityID))
                {
                    case TargetType.None:
                        stateData.SetPendingState(EncounterState.PERFORM_ACTION);
                        break;
                    case TargetType.Enemy:
                        stateData.SetPendingState(EncounterState.CHOOSE_TARGET);
                        break;
                    case TargetType.Ally:
                        stateData.SetPendingState(EncounterState.CHOOSE_TARGET);
                        break;
                }
                break;
            case EncounterState.CHOOSE_TARGET:
                stateData.SetPendingState(EncounterState.PERFORM_ACTION);
                break;
            case EncounterState.CANCEL_ACTION:
                stateData.SetPendingState(EncounterState.CHOOSE_ACTION);
                break;
            case EncounterState.PERFORM_ACTION:
                if (stateData.AreActionPointsAvailable())
                {
                    stateData.SetPendingState(EncounterState.SELECT_CURRENT_CHARACTER);
                }
                else
                {
                    stateData.SetPendingState(EncounterState.DESELECT_CURRENT_CHARACTER);
                }
                break;
            case EncounterState.DESELECT_CURRENT_CHARACTER:
                stateData.PopCurrentCharacter();
                stateData.SetPendingState(EncounterState.UPDATE);
                break;
            case EncounterState.UPDATE:
                //if the timeline is empty, time for a new round
                if (stateData.IsTimelineEmpty() || stateData.IsOpposingTeamDead())
                {
                    stateData.IncrementTurnCount();
                    stateData.SetPendingState(EncounterState.BUILD_QUEUES);
                }
                //if there are still characters in the timeline, keep processing the round
                else
                {
                    stateData.SetPendingState(EncounterState.TEAM_UPDATED);
                    stateData.SetPendingState(EncounterState.SELECT_CURRENT_CHARACTER);
                }
                break;
            case EncounterState.DONE:
                stateData.SetPendingState(EncounterState.DONE);
                break;
            default:
                Debug.Log("No state transition available!");
                break;
        }

        stateData.BroadcastState();
    }
}
