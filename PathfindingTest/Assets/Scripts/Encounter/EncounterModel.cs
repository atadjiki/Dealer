using System.Collections;
using UnityEditor;
using UnityEngine;
using static Constants;

public class EncounterModel : MonoBehaviour
{
    public delegate void EncounterStateDelegate(EncounterState State);
    public static EncounterStateDelegate OnStateChanged;

    private EncounterStateData _state;

    public void Enter()
    {
        //make a fresh state data for the encounter
        _state = EncounterStateData.Build();

        HandleState(EncounterState.INIT);
    }

    public void AddCharacter(TeamID team, CharacterComponent character)
    {
        //Add the character to our state data
        _state.CharacterMap[team].Add(character);
    }

    public void HandleState(EncounterState pendingState)
    {
        switch (pendingState)
        {
            case EncounterState.INIT:
                Init();
                break;
            case EncounterState.SETUP_START:
                SetupStart();
                break;
            case EncounterState.SETUP_COMPLETE:
                SetupComplete();
                break;
            case EncounterState.BUILD_QUEUES:
                BuildQueues();
                break;
            case EncounterState.CHECK_CONDITIONS:
                CheckConditions();
                break;
            case EncounterState.SELECT_CURRENT_CHARACTER:
                SelectCurrentCharacter();
                break;
            case EncounterState.TEAM_UPDATED:
                TeamUpdated();
                break;
            case EncounterState.CHOOSE_ACTION:
                ChooseAction();
                break;
            case EncounterState.CHOOSE_TARGET:
                ChooseTarget();
                break;
            case EncounterState.CANCEL_ACTION:
                CancelAction();
                break;
            case EncounterState.PERFORM_ACTION:
                PerformAction();
                break;
            case EncounterState.DESELECT_CURRENT_CHARACTER:
                DeselectCurrentCharacter();
                break;
            case EncounterState.UPDATE:
                UpdateEncounter();
                break;
            case EncounterState.DONE:
                Done();
                break;
            default:
                Debug.Log("No state transition available!");
                break;
        }

        BroadcastState();
    }

    //State transition functions:
    private void Init()
    {
        SetPendingState(EncounterState.SETUP_START);
    }

    private void SetupStart()
    {
        SetPendingState(EncounterState.SETUP_COMPLETE);
    }

    private void SetupComplete()
    {
        SetPendingState(EncounterState.BUILD_QUEUES);
    }

    private void BuildQueues()
    {
        _state.BuildTimeline();

        SetPendingState(EncounterState.CHECK_CONDITIONS);
    }

    private void CheckConditions()
    {
        //if any queues are empty, then we are done with the encounter
        if (_state.AreAnyTeamsDead())
        {
            //call delegate
            SetPendingState(EncounterState.DONE);
            return;
        }

        SetPendingState(EncounterState.SELECT_CURRENT_CHARACTER);
    }

    private void Done()
    {
        SetPendingState(EncounterState.DONE);
    }

    private void TeamUpdated()
    {
        SetPendingState(EncounterState.SELECT_CURRENT_CHARACTER);
    }

    private void SelectCurrentCharacter()
    {
        if (IsCurrentCharacterAlive())
        {
            SetPendingState(EncounterState.CHOOSE_ACTION);
        }
        else
        {
            SetPendingState(EncounterState.DESELECT_CURRENT_CHARACTER);
        }
    }

    private void ChooseAction()
    {
        //check if we need to choose a target for this action
        AbilityID abilityID = GetActiveAbility();

        switch (GetTargetType(abilityID))
        {
            case TargetType.None:
                SetPendingState(EncounterState.PERFORM_ACTION);
                break;
            case TargetType.Enemy:
                SetPendingState(EncounterState.CHOOSE_TARGET);
                break;
            case TargetType.Ally:
                SetPendingState(EncounterState.CHOOSE_TARGET);
                break;
        }
    }

    private void ChooseTarget()
    {
        SetPendingState(EncounterState.PERFORM_ACTION);
    }

    private void CancelAction()
    {
        SetPendingState(EncounterState.CHOOSE_ACTION);
    }

    private void PerformAction()
    {
        CharacterComponent currentCharacter;
        if (GetCurrentCharacter(out currentCharacter))
        {
            if (currentCharacter.HasActionPoints())
            {
                SetPendingState(EncounterState.SELECT_CURRENT_CHARACTER);
            }
            else
            {
                SetPendingState(EncounterState.DESELECT_CURRENT_CHARACTER);
            }
        }
    }

    private void DeselectCurrentCharacter()
    {
        PopCurrentCharacter();

        SetPendingState(EncounterState.UPDATE);
    }

    private void UpdateEncounter()
    {
        //if the timeline is empty, time for a new round
        if (_state.IsTimelineEmpty() || _state.IsOpposingTeamDead())
        {
            IncrementTurnCount();
            _state.ResetTeam();
            SetPendingState(EncounterState.BUILD_QUEUES);
        }
        //if there are still characters in the timeline, keep processing the round
        else
        {
            _state.IncrementTeam();
            SetPendingState(EncounterState.TEAM_UPDATED);
            SetPendingState(EncounterState.SELECT_CURRENT_CHARACTER);
        }
    }

    //Helpers

    private void SetPendingState(EncounterState state)
    {
        if (state != _state.CurrentState)
        {
            _state.CurrentState = state;
        }
    }

    private void BroadcastState()
    {
        Debug.Log("Encounter State: " + _state.CurrentState.ToString());

        if (OnStateChanged != null)
        {
            OnStateChanged.Invoke(_state.CurrentState);
        }
    }

    public void CancelActiveAbility()
    {
        CharacterComponent currentCharacter;
        if (GetCurrentCharacter(out currentCharacter))
        {
            currentCharacter.ResetForTurn();

            SetPendingState(EncounterState.CANCEL_ACTION);
        }
    }

    public bool GetCurrentCharacter(out CharacterComponent character)
    {
        character = _state.GetCurrentCharacter();

        if (character != null)
        {
            return true;
        }
        else
        {
            Debug.LogError("Current character was null!");
            return false;
        }
    }

    public void PopCurrentCharacter()
    {
        CancelActiveAbility();
        _state.PopCurrentCharacter();
    }

    public Vector3 GetCurrentCharacterLocation()
    {
        CharacterComponent currentCharacter;
        if (GetCurrentCharacter(out currentCharacter))
        {
            return currentCharacter.GetWorldLocation();
        }

        return Vector3.zero;
    }

    public bool IsCurrentCharacterAlive()
    {
        return _state.IsCurrentCharacterAlive();
    }

    public int IncrementTurnCount()
    {
        Debug.Log("Turn Count: " + (_state.TurnCount + 1));
        return _state.TurnCount++;
    }

    public AbilityID GetActiveAbility()
    {
        CharacterComponent currentCharacter;
        if (GetCurrentCharacter(out currentCharacter))
        {
            return currentCharacter.GetActiveAbility();
        }

        return AbilityID.NONE;
    }

    public void SetActiveAbility(AbilityID ability)
    {
        CharacterComponent currentCharacter;
        if (GetCurrentCharacter(out currentCharacter))
        {
            currentCharacter.SetActiveAbility(ability);
        }
    }

    public void SetActiveDestination(Vector3 destination)
    {
        CharacterComponent currentCharacter;
        if (GetCurrentCharacter(out currentCharacter))
        {
            currentCharacter.SetActiveDestination(destination);
        }
    }

    public EncounterState GetState()
    {
        return _state.CurrentState;
    }

    //Debug 
    private void OnDrawGizmosSelected()
    {
        if (Application.isPlaying)
        {
            CharacterComponent currentCharacter = _state.GetCurrentCharacter();

            if (currentCharacter != null)
            {
                Gizmos.color = Color.green;
                Handles.color = Color.green;
                Handles.Label(currentCharacter.GetWorldLocation(), currentCharacter.GetID().ToString());
            }
        }
    }

    private void OnGUI()
    {
        if (Application.isPlaying)
        {
            int TextWidth = 200;
            GUI.contentColor = Color.green;
            GUI.Label(new Rect(Screen.width - TextWidth, 10, TextWidth, 22), _state.CurrentState.ToString());
        }
    }
}
