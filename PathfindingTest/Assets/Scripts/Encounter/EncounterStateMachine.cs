using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static Constants;

[Serializable]
public struct EncounterStateData
{
    public Dictionary<TeamID, List<CharacterComponent>> CharacterMap;

    public Dictionary<TeamID, Queue<CharacterComponent>> TeamQueues;

    public EncounterState CurrentState;
    public TeamID CurrentTeam;

    public int TurnCount;

    public bool Busy;

    public CharacterComponent GetCurrentCharacter()
    {
        if(TeamQueues != null)
        {
            if(TeamQueues[CurrentTeam] != null)
            {
                if(TeamQueues[CurrentTeam].Count > 0)
                {
                    return TeamQueues[CurrentTeam].Peek();
                }
            }
        }

        return null;
    }

    public void PopCurrentCharacter()
    {
        if (TeamQueues != null)
        {
            if (TeamQueues[CurrentTeam] != null)
            {
                if (TeamQueues[CurrentTeam].Count > 0)
                {
                    TeamQueues[CurrentTeam].Dequeue();
                }
            }
        }
    }

    public bool IsCurrentCharacterAlive()
    {
        return GetCurrentCharacter().IsAlive();
    }

    public void BuildAllTeamQueues()
    {
        foreach(TeamID team in CharacterMap.Keys)
        {
            BuildTeamQueue(team);
        }
    }

    public void BuildTeamQueue(TeamID team)
    {
        TeamQueues[team].Clear();

        string debugString = team.ToString() + " Queue: ";

        foreach (CharacterComponent character in CharacterMap[team])
        {
            if(character.IsAlive())
            {
                TeamQueues[team].Enqueue(character);
                debugString += character.GetID().ToString() + " ";
            }
        }

        Debug.Log(debugString);
    }

    public bool AreAnyTeamsDead()
    {
        foreach(TeamID team in CharacterMap.Keys)
        {
            if(CharacterMap[team].Count > 0)
            {
                if (IsTeamDead(team))
                {
                    return true;
                }
            }
        }

        return false;
    }

    public bool IsTeamDead(TeamID team)
    {
        foreach(CharacterComponent character in CharacterMap[team])
        {
            if(character.IsAlive())
            {
                return false;
            }
        }

        Debug.Log("Team " + team.ToString() + " is dead");
        return true;
    }

    public List<CharacterComponent> GetCharactersInTeam(TeamID team)
    {
        return CharacterMap[team];
    }

    public static EncounterStateData Build()
    {
        return new EncounterStateData()
        {
            CharacterMap = new Dictionary<TeamID, List<CharacterComponent>>()
            {
                { TeamID.PLAYER, new List<CharacterComponent>() },
                { TeamID.ENEMY,  new List<CharacterComponent>() }
            },

            TeamQueues = new Dictionary<TeamID, Queue<CharacterComponent>>()
            {
                { TeamID.PLAYER, new Queue<CharacterComponent>() },
                { TeamID.ENEMY,  new Queue<CharacterComponent>() },

            },

            CurrentState = EncounterState.NONE,
            CurrentTeam = TeamID.PLAYER,

            TurnCount = 0,

            Busy = false
        };
    }
}

public class EncounterStateMachine: MonoBehaviour
{
    //event handling
    public delegate void EncounterStateDelegate(EncounterState State);
    public static EncounterStateDelegate OnStateChanged;

    public delegate void EncounterAbilityDelegate(AbilityID ID, object data);
    public static EncounterAbilityDelegate OnAbilityChosen;

    //who should we spawn, etc
    [SerializeField] private EncounterSetupData SetupData;

    //private vars
    private EncounterStateData _state;

    private void Awake()
    {
        //check if we have pathfinding data?
        EnvironmentUtil.Scan();

        OnStateChanged += StateChangeCallback;

        OnAbilityChosen += AbilityChosenCallback;

        HandleState(EncounterState.INIT);
    }

    private void OnDestroy()
    {
        OnStateChanged -= StateChangeCallback;
        OnAbilityChosen -= AbilityChosenCallback;
    }

    public void AbilityChosenCallback(AbilityID ability, object data)
    {
        SetActiveAbility(ability);

        switch (ability)
        {
            case AbilityID.MOVE_FULL:
            case AbilityID.MOVE_HALF:
            {
                Vector3 destination = ((Vector3)data);
                SetActiveDestination(destination);
                HandleState(EncounterState.CHOOSE_ACTION);
                break;
            }
            default:
                break;
        }
    }

    public void StateChangeCallback(EncounterState state)
    {
        StartCoroutine(Coroutine_StateChangeCallback(state));
    }

    private IEnumerator Coroutine_StateChangeCallback(EncounterState state)
    {
        //yield return new WaitForSecondsRealtime(1.5f);

        switch (state)
        {
            case EncounterState.SETUP_COMPLETE:
            {
                //BroadcastCharacterEvent(CharacterEvent.UPDATE);
                break;
            }
            case EncounterState.SELECT_CURRENT_CHARACTER:
            {
                EnvironmentUtil.Scan();

                CharacterComponent currentCharacter;
                GetCurrentCharacter(out currentCharacter);

                CameraRig.Instance.Follow(currentCharacter);

                if (currentCharacter.IsAlive())
                {
                    //TODO currentCharacter.HandleEvent(CharacterEvent.SELECTED);
                }
                break;
            }
            case EncounterState.TEAM_UPDATED:
            {
                yield return new WaitForSecondsRealtime(1.5f);
                break;
            }
            case EncounterState.DESELECT_CURRENT_CHARACTER:
            {
                CameraRig.Instance.Unfollow();
                break;
            }
            case EncounterState.CHOOSE_ACTION:
            {
                EncounterUtil.CreateTileSelector();
                EncounterUtil.CreatePathDisplay(GetCurrentCharacterLocation());
                EncounterUtil.CreateMovementRadius();
                Debug.Log("Waiting for player input");
                //if it's the player's turn, wait for input
                //if enemy turn, let the ai choose an ability and transition
                yield break;
            }
            case EncounterState.CHOOSE_TARGET:
            {
                //if it's the player's turn, wait for input
                Debug.Log("Waiting for player input");
                //if enemy turn, let the ai choose a target
                yield break;
            }
            case EncounterState.PERFORM_ACTION:
            {
                CharacterComponent currentCharacter;
                GetCurrentCharacter(out currentCharacter);
                yield return PerformAbility(currentCharacter);
                //TODO BroadcastCharacterEvent(CharacterEvent.UPDATE);
                break;
            }
            default:
                break;
        }

        HandleState(state);

        yield return null;
    }

    public void HandleState(EncounterState pendingState)
    {
        StartCoroutine(Coroutine_HandleState(pendingState));
    }

    private IEnumerator Coroutine_HandleState(EncounterState pendingState)
    {
        _state.Busy = true;

        switch (pendingState)
        {
            case EncounterState.INIT:
                yield return Coroutine_HandleInit();
                break;
            case EncounterState.SETUP_COMPLETE:
                yield return Coroutine_SetupComplete();
                break;
            case EncounterState.BUILD_QUEUES:
                yield return Coroutine_BuildQueues();
                break;
            case EncounterState.CHECK_CONDITIONS:
                yield return Coroutine_CheckConditions();
                break;
            case EncounterState.SELECT_CURRENT_CHARACTER:
                yield return Coroutine_SelectCurrentCharacter();
                break;
            case EncounterState.TEAM_UPDATED:
                yield return Coroutine_TeamUpdated();
                break;
            case EncounterState.CHOOSE_ACTION:
                yield return Coroutine_ChooseAction();
                break;
            case EncounterState.CHOOSE_TARGET:
                yield return Coroutine_ChooseTarget();
                break;
            case EncounterState.CANCEL_ACTION:
                yield return Coroutine_CancelAction();
                break;
            case EncounterState.PERFORM_ACTION:
                yield return Coroutine_PerformAction();
                break;
            //case EncounterState.DESELECT_CURRENT_CHARACTER:
            //    yield return Coroutine_DeselectCurrentCharacter();
            //    break;
            //case EncounterState.UPDATE:
            //    yield return Coroutine_UpdateEncounter();
            //    break;
            case EncounterState.DONE:
                yield return Coroutine_Done();
                break;
            default:
                Debug.Log("No state transition available!");
                break;
        }

        BroadcastState();

        _state.Busy = false;

        yield return null;
    }

    private IEnumerator Coroutine_HandleInit()
    {
        //make a fresh state data for the encounter
        _state = EncounterStateData.Build();

        //spawn our characters 
        foreach(EncounterTeamData teamData in SetupData.Teams)
        {
            foreach(CharacterID ID in teamData.Characters)
            {
                CharacterData characterData = ResourceUtil.GetCharacterData(ID);

                if (characterData != null)
                {
                    CharacterComponent character = CharacterUtil.BuildCharacterObject(characterData, this.transform);

                    yield return character.Coroutine_Setup(characterData);

                    Vector3 destination = EnvironmentUtil.GetClosestTileWithCover(character.GetWorldLocation());
                    character.Teleport(destination);
                    Debug.Log("Character " + characterData.ID + " spawned at " + destination.ToString());

                    //Add the character to our state data
                    _state.CharacterMap[teamData.Team].Add(character);
                }
            }
        }

        //kick off a camera rig
        EncounterUtil.CreateCameraRig();

        SetPendingState(EncounterState.SETUP_COMPLETE);
    }

    private IEnumerator Coroutine_SetupComplete()
    {
        SetPendingState(EncounterState.BUILD_QUEUES);
        yield return null;
    }

    private IEnumerator Coroutine_BuildQueues()
    {
        _state.BuildAllTeamQueues();

        SetPendingState(EncounterState.CHECK_CONDITIONS);

        yield return null;
    }

    private IEnumerator Coroutine_CheckConditions()
    {
        //if any queues are empty, then we are done with the encounter
       if(_state.AreAnyTeamsDead())
        {
            //call delegate
            SetPendingState(EncounterState.DONE);
            yield break;
        }

        yield return null;

        SetPendingState(EncounterState.SELECT_CURRENT_CHARACTER);
    }

    private IEnumerator Coroutine_Done()
    {
        SetPendingState(EncounterState.DONE);
        yield return null;
    }

    private IEnumerator Coroutine_TeamUpdated()
    {
        SetPendingState(EncounterState.SELECT_CURRENT_CHARACTER);
        yield return null;
    }

    private IEnumerator Coroutine_SelectCurrentCharacter()
    {
        if (IsCurrentCharacterAlive())
        {
            SetPendingState(EncounterState.CHOOSE_ACTION);
        }
        else
        {
            SetPendingState(EncounterState.DESELECT_CURRENT_CHARACTER);
        }

        yield return null;
    }

    private IEnumerator Coroutine_ChooseAction()
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

        yield return null;
    }

    private IEnumerator Coroutine_ChooseTarget()
    {
        SetPendingState(EncounterState.PERFORM_ACTION);
        yield return null;
    }

    public IEnumerator PerformAbility(CharacterComponent caster)
    {
        AbilityID abilityID = GetActiveAbility();

        //TODO
        //string casterName = GetDisplayString(caster.GetID());
        //string abilityString = GetEventString(abilityID);

        //Debug.Log(casterName + " " + abilityString + "...");

        //TODO caster.PlayAudio(CharacterAudioType.Confirm);

        //UnfollowCharacter();

        //_canvas.ShowEventBanner(casterName + " " + abilityString + "...");
        //yield return new WaitForSeconds(DEFAULT_WAIT_TIME);

        yield return caster.Coroutine_PerformAbility();
    }

    private IEnumerator Coroutine_CancelAction()
    {
        SetPendingState(EncounterState.CHOOSE_ACTION);
        yield return null;
    }

    private IEnumerator Coroutine_PerformAction()
    {
        CharacterComponent currentCharacter;
        if(GetCurrentCharacter(out currentCharacter))
        {
            //TODO currentCharacter.DecrementActionPoints(GetAbilityCost(GetActiveAbility()));

            if (currentCharacter.HasActionPoints())
            {
                SetPendingState(EncounterState.SELECT_CURRENT_CHARACTER);
            }
            else
            {
                SetPendingState(EncounterState.DESELECT_CURRENT_CHARACTER);
            }
        }

        yield return null;
    }

    private IEnumerator Coroutine_DeselectCurrentCharacter()
    {
        PopCurrentCharacter();

        SetPendingState(EncounterState.UPDATE);
        yield return null;
    }

    //private IEnumerator Coroutine_UpdateEncounter()
    //{
    //    //if the timeline is empty, time for a new round
    //    if (IsTimelineEmpty() || IsOpposingTeamDead())
    //    {
    //        IncrementTurnCount();
    //        ResetTeam();
    //        SetPendingState(EncounterState.BUILD_QUEUES);
    //    }
    //    //if there are still characters in the timeline, keep processing the round
    //    else
    //    {
    //        IncrementTeam();
    //        SetPendingState(EncounterState.TEAM_UPDATED);
    //        SetPendingState(EncounterState.SELECT_CURRENT_CHARACTER);
    //    }

    //    yield return null;
    //}

    private void BroadcastState()
    {
        Debug.Log("Encounter State: " + _state.CurrentState.ToString());

        if (OnStateChanged != null)
        {
            OnStateChanged.Invoke(_state.CurrentState);
        }
    }

    public AbilityID GetActiveAbility()
    {
        CharacterComponent currentCharacter;
        if(GetCurrentCharacter(out currentCharacter))
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

    //State interface

    public void PopCurrentCharacter()
    {
        CancelActiveAbility();
        _state.PopCurrentCharacter();

    }

    public void CancelActiveAbility()
    {
        CharacterComponent currentCharacter;
        if(GetCurrentCharacter(out currentCharacter))
        {
            currentCharacter.ResetForTurn();

            SetPendingState(EncounterState.CANCEL_ACTION);
        }
    }

    public bool GetCurrentCharacter(out CharacterComponent character)
    {
        character = _state.GetCurrentCharacter();

        if(character != null)
        {
            return true;
        }
        else
        {
            Debug.LogError("Current character was null!");
            return false;
        }
    }

    public Vector3 GetCurrentCharacterLocation()
    {
        CharacterComponent currentCharacter;
        if(GetCurrentCharacter(out currentCharacter))
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

    //state
    public EncounterState GetState()
    {
        return _state.CurrentState;
    }

    private void SetPendingState(EncounterState state)
    {
        if (state != _state.CurrentState)
        {
            _state.CurrentState = state;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (Application.isPlaying)
        {
            CharacterComponent currentCharacter = _state.GetCurrentCharacter();

            if(currentCharacter != null)
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
