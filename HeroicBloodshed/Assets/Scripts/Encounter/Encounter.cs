using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

using static Constants;

public class Encounter : MonoBehaviour
{
    //event handling
    public delegate void EncounterStateDelegate(EncounterState encounterState);
    public EncounterStateDelegate OnStateChanged;

    //setup data
    private EncounterSetupData _setupData;

    //collections
    //when setup is performed, store spawned characters
    private Dictionary<TeamID, List<CharacterComponent>> _characterMap;

    //create a queue for each team each combat loop
    private Dictionary<TeamID, Queue<CharacterComponent>> _queues;

    private EncounterState _pendingState = EncounterState.SETUP;

    private bool _busy = false;

    private int _turnCount;

    private TeamID _currentTeam;

    private CinemachineVirtualCamera _virtualCamera;

    private Transform _defaultCameraFollow;

    private void Awake()
    {
        Debug.Log("Encounter created " + this.name);
    }
    //
    //state machine

    public void TransitionState()
    {
        if(!_busy)
        {
            StartCoroutine(Coroutine_TransitionState());
        }
        else
        {
            Debug.Log("Encounter is busy");
        }

    }

    private IEnumerator Coroutine_TransitionState()
    {
        _busy = true;

        Debug.Log("Handling state " + _pendingState);

        switch(_pendingState)
        {
            case EncounterState.SETUP:
                yield return Coroutine_Setup();
                break;
            case EncounterState.BUILD_QUEUES:
                yield return Coroutine_BuildQueues();
                break;
            case EncounterState.CHECK_CONDITIONS:
                yield return Coroutine_CheckConditions();
                break;
            case EncounterState.PROCESS_TURN:
                yield return Coroutine_ProcessTurn();
                break;
            case EncounterState.SELECT_CURRENT_CHARACTER:
                yield return Coroutine_SelectCurrentCharacter();
                break;
            case EncounterState.WAIT_FOR_PLAYER_INPUT:
                yield return Coroutine_WaitForInput();
                break;
            case EncounterState.CHOOSE_AI_ACTION:
                yield return Coroutine_ChooseAIAction();
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

        yield return null;
    }

    //state behaviors

    private IEnumerator Coroutine_Setup()
    {
        Debug.Log("Performing Setup");

        _virtualCamera = _setupData.VirtualCamera;
        _defaultCameraFollow = _virtualCamera.Follow;

        _turnCount = 0;
        _currentTeam = TeamID.Player; //the player always goes first

        _characterMap = new Dictionary<TeamID, List<CharacterComponent>>();

        foreach (EncounterTeamData teamData in _setupData.Teams)
        {
            _characterMap.Add(teamData.Team, new List<CharacterComponent>());

            //spawn characters for each team 
            foreach (CharacterID characterID in teamData.Characters)
            {
                //see if we have a marker available to spawn them in
                foreach (CharacterMarker marker in teamData.Markers)
                {
                    if (marker.IsOccupied() == false)
                    {
                        marker.SetOccupied(true);

                        GameObject characterObject = EncounterHelper.CreateCharacterObject(teamData.Team + "_" + characterID, marker.transform);
                        CharacterComponent characterComponent = EncounterHelper.AddComponentByTeam(characterID, characterObject);

                        _characterMap[teamData.Team].Add(characterComponent);

                        break;
                    }
                }
            }
        }

        yield return EncounterHelper.SpawnCharacters(this);

        SetState(EncounterState.BUILD_QUEUES);
    }

    private IEnumerator Coroutine_BuildQueues()
    {
        Debug.Log("Building Queues");

        _queues = new Dictionary<TeamID, Queue<CharacterComponent>>();

        foreach (TeamID team in _characterMap.Keys)
        {
            if (_queues.ContainsKey(team) == false)
            {
                _queues.Add(team, new Queue<CharacterComponent>()); //create a new queue

                foreach (CharacterComponent characterComponent in _characterMap[team])
                {
                    if (characterComponent.IsAlive())
                    {
                        _queues[team].Enqueue(characterComponent);
                    }
                }
            }
        }

        SetState(EncounterState.CHECK_CONDITIONS);

        yield return null;
    }

    private IEnumerator Coroutine_CheckConditions()
    {
        Debug.Log("Checking Conditions");

        //if any queues are empty, then we are done with the encounter
        foreach (Queue<CharacterComponent> TeamQueue in _queues.Values)
        {
            if (TeamQueue.Count == 0)
            {
                //call delegate
                SetState(EncounterState.DONE);
                yield break;
            }
        }

        SetState(EncounterState.PROCESS_TURN);
        yield return null;
    }

    private IEnumerator Coroutine_Done()
    {
        Debug.Log("Encounter done");

        SetState(EncounterState.DONE);
        yield return null;
    }

    private IEnumerator Coroutine_ProcessTurn()
    {
        Debug.Log("Processing Turn");

        SetState(EncounterState.SELECT_CURRENT_CHARACTER);
        yield return null;
    }

    private IEnumerator Coroutine_SelectCurrentCharacter()
    {
        Debug.Log("Selecting Current Character");

        SetState(EncounterState.CHOOSE_AI_ACTION);
        yield return null;
    }

    private IEnumerator Coroutine_WaitForInput()
    {
        Debug.Log("Waiting For Player Input");

        yield return null;
    }

    private IEnumerator Coroutine_ChooseAIAction()
    {
        Debug.Log("Choose AI Action");

        SetState(EncounterState.PERFORM_ACTION);
        yield return null;
    }

    private IEnumerator Coroutine_PerformAction()
    {
        Debug.Log("Performing Action");

        SetState(EncounterState.DESELECT_CURRENT_CHARACTER);
        yield return null;
    }

    private IEnumerator Coroutine_DeselectCurrentCharacter()
    {
        Debug.Log("Deselecting Current Character");

        SetState(EncounterState.UPDATE);
        yield return null;
    }

    private IEnumerator Coroutine_UpdateEncounter()
    {
        Debug.Log("Updating Encounter");

        SetState(EncounterState.BUILD_QUEUES);
        yield return null;
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

        return _currentTeam;
    }

    //getters/setters

    //data
    public void SetSetupData(EncounterSetupData setupData) { _setupData = setupData; }

    //state
    public EncounterState GetState() { return _pendingState; }

    private void SetState(EncounterState state)
    {
        Debug.Log("New pending state: " + _pendingState + " to " + state);

        _pendingState = state;

        if (OnStateChanged != null)
        {
            OnStateChanged.Invoke(_pendingState);
        }
    }
    //characters
    public CharacterComponent GetCurrentCharacter() { return _queues[_currentTeam].Peek(); }

    public List<CharacterComponent> GetAllCharactersInTeam(TeamID teamID) { return _characterMap[teamID]; }

    public List<CharacterComponent> GetAllCharactersInTeamQueue(TeamID teamID) { return new List<CharacterComponent>(_queues[teamID].ToArray()); }

    public void PopCurrentCharacter() { _queues[_currentTeam].Dequeue(); }

    //teams
    public bool IsCurrentTeamQueueEmpty() { return IsQueueEmpty(_currentTeam); }

    public TeamID GetCurrentTeam() { return _currentTeam; }

    //turns
    public int GetTurnCount() { return _turnCount; }

    public int IncrementTurnCount() { return _turnCount++; }

    //camera
    public void ToggleCamera(bool flag) { _virtualCamera.enabled = flag; }

    public void SetCameraFollow(Transform target) { _virtualCamera.Follow = target; }

    public void ResetCameraFollow() { SetCameraFollow(_defaultCameraFollow); }
}
