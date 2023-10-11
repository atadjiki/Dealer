using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static Constants;

public class EncounterModel : MonoBehaviour
{
    //event handling
    public delegate void EncounterStateDelegate();
    public EncounterStateDelegate OnStateChanged;

    //setup data
    private EncounterSetupData _setupData;

    //collections
    //when setup is performed, store spawned characters
    private Dictionary<TeamID, List<CharacterComponent>> _characterMap;

    //create a queue for each team each combat loop
    private Dictionary<TeamID, Queue<CharacterComponent>> _queues;

    private EncounterState _pendingState = EncounterState.INIT;

    private bool _busy = false;

    private int _turnCount;

    private TeamID _currentTeam;

    private AbilityID _activeAbility = AbilityID.NONE;
    private CharacterComponent _activeTarget = null;

    private Transform _cameraFollow;

    //
    //state machine

    public IEnumerator Setup(EncounterSetupData setupData)
    {
        Debug.Log("Encounter init " + this.name);

        _setupData = setupData;

        yield return new WaitForFixedUpdate();

        _turnCount = 1;
        _currentTeam = TeamID.Player; //the player always goes first

        _characterMap = new Dictionary<TeamID, List<CharacterComponent>>();

        _cameraFollow = _setupData.CameraFollowTarget;

        foreach (EncounterTeamData teamData in _setupData.Teams)
        {
            _characterMap.Add(teamData.Team, new List<CharacterComponent>());

            //spawn characters for each team 
            foreach (CharacterID characterID in teamData.Characters)
            {
                CharacterComponent characterComponent = EnvironmentManager.Instance.SpawnCharacter(teamData.Team, characterID);

                if(characterComponent != null)
                {
                    _characterMap[teamData.Team].Add(characterComponent);
                }
            }
        }
    }

    public void TransitionState()
    {
        if(!_busy)
        {
            StartCoroutine(Coroutine_TransitionState());
        }
    }

    private IEnumerator Coroutine_TransitionState()
    {
        _busy = true;

        Debug.Log("Handling state " + _pendingState);

        switch(_pendingState)
        {
            case EncounterState.INIT:
                yield return Coroutine_Init();
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
            case EncounterState.TEAM_UPDATED:
                yield return Coroutine_TeamUpdated();
                break;
            case EncounterState.SELECT_CURRENT_CHARACTER:
                yield return Coroutine_SelectCurrentCharacter();
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
            case EncounterState.DESELECT_CURRENT_CHARACTER:
                yield return Coroutine_DeselectCurrentCharacter();
                break;
            case EncounterState.UPDATE:
                yield return Coroutine_UpdateEncounter();
                break;
            case EncounterState.DONE:
                yield return Coroutine_Done();
                break;
            default:
                Debug.Log("No state transition available!");
                break;
        }

        _busy = false;

        //broadcast state change
        if (OnStateChanged != null)
        {
            OnStateChanged.Invoke();
        }

        yield return null;
    }

    private IEnumerator Coroutine_Init()
    {
        SetPendingState(EncounterState.SETUP_COMPLETE);
        yield return null;
    }

    private IEnumerator Coroutine_SetupComplete()
    {
        SetPendingState(EncounterState.BUILD_QUEUES);
        yield return null;
    }

    private IEnumerator Coroutine_BuildQueues()
    {
        _queues = new Dictionary<TeamID, Queue<CharacterComponent>>();

        foreach (TeamID team in _characterMap.Keys)
        {
            string debugString = "Queue - Team: " + team + "{ ";

            if (_queues.ContainsKey(team) == false)
            {

                _queues.Add(team, new Queue<CharacterComponent>()); //create a new queue

                foreach (CharacterComponent characterComponent in _characterMap[team])
                {
                    if (characterComponent.IsAlive())
                    {
                        debugString += characterComponent.GetID() + ", ";
                        _queues[team].Enqueue(characterComponent);
                    }
                }
                debugString += " }";
            }

            Debug.Log(debugString);
        }

        SetPendingState(EncounterState.CHECK_CONDITIONS);
        yield return null;
    }

    private IEnumerator Coroutine_CheckConditions()
    {
        //if any queues are empty, then we are done with the encounter
        foreach (Queue<CharacterComponent> TeamQueue in _queues.Values)
        {
            if (TeamQueue.Count == 0)
            {
                //call delegate
                SetPendingState(EncounterState.DONE);
                yield break;
            }
        }

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
        if(GetCurrentCharacter().IsAlive())
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

        switch(GetTargetType(abilityID))
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

    private IEnumerator Coroutine_CancelAction()
    {
        SetPendingState(EncounterState.CHOOSE_ACTION);
        yield return null;
    }

    private IEnumerator Coroutine_PerformAction()
    {
        SetPendingState(EncounterState.DESELECT_CURRENT_CHARACTER);
        yield return null;
    }

    private IEnumerator Coroutine_DeselectCurrentCharacter()
    {
        PopCurrentCharacter();

        SetPendingState(EncounterState.UPDATE);
        yield return null;
    }

    private IEnumerator Coroutine_UpdateEncounter()
    {
        if(AreAllQueuesEmpty() || IsOpposingTeamDead())
        { 
            IncrementTurnCount();
            ResetTeam();
            SetPendingState(EncounterState.BUILD_QUEUES);
        }
        //if the current team's queue is empty, it's the next teams turn
        else if(IsCurrentTeamQueueEmpty())
        {
            IncrementTeam();
            SetPendingState(EncounterState.TEAM_UPDATED);
        }
        //if we still have characters left in the current team's queue, do nothing here
        else
        {
            SetPendingState(EncounterState.SELECT_CURRENT_CHARACTER);
        }

        yield return null;
    }

    public bool IsAbilitySelected()
    {
        return GetActiveAbility() != AbilityID.NONE;
    }

    //
    //helpers

    public List<CharacterComponent> GetAllCharacters()
    {
        List<CharacterComponent> result = new List<CharacterComponent>();

        foreach (List<CharacterComponent> teamList in _characterMap.Values)
        {
            result.AddRange(teamList);
        }

        return result;
    }

    public bool IsQueueEmpty(TeamID teamID)
    {
        if (_queues.ContainsKey(teamID)) { return (_queues[teamID].Count == 0); }

        return false;
    }

    public bool AreAllQueuesEmpty()
    {
        foreach(Queue<CharacterComponent> queue in _queues.Values)
        {
            if(queue.Count != 0)
            {
                return false;
            }
        }

        return true;
    }

    public bool IsOpposingTeamDead()
    {
        return IsTeamDead(GetOpposingTeam(GetCurrentTeam()));
    }

    public bool IsTeamDead(TeamID team)
    {
        foreach(CharacterComponent character in GetAllCharactersInTeam(team))
        {
            if(character.IsAlive())
            {
                return false;
            }
        }

        return true;
    }

    public void ResetTeam()
    {
        _currentTeam = TeamID.Player;

        Debug.Log("Current Team: " + _currentTeam);
    }

    public TeamID IncrementTeam()
    {
        switch (_currentTeam)
        {
            case TeamID.Player:
                _currentTeam = TeamID.Enemy;
                break;
            case TeamID.Enemy:
                _currentTeam = TeamID.None;
                break;
        }

        Debug.Log("Current Team: " + _currentTeam);

        return _currentTeam;
    }

    //getters/setters

    //ability
    public void SetActiveAbility(AbilityID abilityID)
    {
        Debug.Log("Selected ability: " + abilityID);
        _activeAbility = abilityID;
    }

    public AbilityID GetActiveAbility()
    {
        return _activeAbility;
    }

    public void SetTarget(CharacterComponent target)
    {
        if(target != null)
        {
            Debug.Log("Selected target: " + target.gameObject.name);
            target.ToggleHighlight(true);

        }
        _activeTarget = target;
    }

    public CharacterComponent GetActiveTarget()
    {
        return _activeTarget;
    }

    public void CancelActiveAbility()
    {
        _activeAbility = AbilityID.NONE;
        _activeTarget = null;

        SetPendingState(EncounterState.CANCEL_ACTION);
    }

    //control
    public bool IsCurrentTeamCPU()
    {
        return (GetCurrentTeam() == TeamID.Enemy || _setupData.IsPlayerCPU);
 
    }

    //state
    public EncounterState GetState() { return _pendingState; }

    private void SetPendingState(EncounterState state)
    {
        _pendingState = state;
    }
    //characters
    public CharacterComponent GetCurrentCharacter()
    {
        if(_queues[_currentTeam].Count > 0)
        {
            return _queues[_currentTeam].Peek();
        }
        else
        {
            return null;
        }

    }

    public bool AreTargetsAvailable()
    {
        return GetTargetCandidates().Count > 0;
    }

    public List<CharacterComponent> GetTargetCandidates()
    {
        TeamID opposingTeam = GetOpposingTeam(GetCurrentTeam());

        List<CharacterComponent> candidates = new List<CharacterComponent>();

        foreach (CharacterComponent character in GetAllCharactersInTeam(opposingTeam))
        {
            if(character.IsAlive())
            {
                candidates.Add(character);
            }
        }

        return candidates;
    }

    public List<CharacterComponent> GetAllCharactersInTeam(TeamID teamID) { return _characterMap[teamID]; }

    public List<CharacterComponent> GetAllCharactersInTeamQueue(TeamID teamID) { return new List<CharacterComponent>(_queues[teamID].ToArray()); }

    public void PopCurrentCharacter()
    {
        CharacterComponent dequedCharacter = _queues[_currentTeam].Dequeue();
        CancelActiveAbility();
    }

    //teams
    public bool IsCurrentTeamQueueEmpty() { return IsQueueEmpty(_currentTeam); }

    public TeamID GetCurrentTeam() { return _currentTeam; }

    //turns
    public int GetTurnCount() { return _turnCount; }

    public Transform GetCameraFollow()
    {
        return _cameraFollow;
    }

    public int IncrementTurnCount()
    {
        Debug.Log("Turn Count: " + (_turnCount + 1));
        return _turnCount++;
    }

    public int GetDeadCount(TeamID team)
    {
        int count = 0;

        foreach(CharacterComponent character in GetAllCharactersInTeam(team))
        {
            if(character.IsDead())
            {
                count++;
            }
        }

        return count;
    }

    public bool DidPlayerWin()
    {
        if(IsTeamDead(TeamID.Player))
        {
            return false;
        }

        return true;
    }
}
