using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

using static Constants;

[Serializable]
public class Encounter : MonoBehaviour
{
    //event handling
    public delegate void EncounterStateDelegate(EncounterState encounterState);
    public EncounterStateDelegate OnStateChanged;

    //setup data
    EncounterSetupData _setupData;

    public void SetSetupData(EncounterSetupData setupData) { _setupData = setupData; }

    //collections
    //when setup is performed, store spawned characters
    private Dictionary<TeamID, List<CharacterComponent>> _characterMap;

    //create a queue for each team each combat loop
    private Dictionary<TeamID, Queue<CharacterComponent>> _queues;

    private EncounterState _pendingState = EncounterState.SETUP;

    public EncounterState GetState() { return _pendingState; }

    private bool _busy = false;

    private void SetState(EncounterState state)
    {
        Debug.Log("Transitioning from " + _pendingState + " to " + state);

        _pendingState = state;

        if (OnStateChanged != null)
        {
            OnStateChanged.Invoke(_pendingState);
        }
    }

    private int _turnCount;

    private TeamID _currentTeam;

    private CinemachineVirtualCamera _virtualCamera;

    private Transform _defaultCameraFollow;

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
            default:
                Debug.Log("No state transition available!");
                break;
        }

        _busy = false;

        yield return null;
    }

    //state behavior
    private IEnumerator Coroutine_Setup()
    {
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
        SetState(EncounterState.DONE);
        yield return null;
    }

    private IEnumerator Coroutine_ProcessTurn()
    {
        SetState(EncounterState.SELECT_CURRENT_CHARACTER);
        yield return null;
    }

    private IEnumerator Coroutine_SelectCurrentCharacter()
    {
        SetState(EncounterState.CHOOSE_AI_ACTION);
        yield return null;
    }

    private IEnumerator Coroutine_WaitForInput()
    {
        yield return null;
    }

    private IEnumerator Coroutine_ChooseAIAction()
    {
        SetState(EncounterState.PERFORM_ACTION);
        yield return null;
    }

    private IEnumerator Coroutine_PerformAction()
    {
        SetState(EncounterState.DESELECT_CURRENT_CHARACTER);
        yield return null;
    }

    private IEnumerator Coroutine_DeselectCurrentCharacter()
    {
        SetState(EncounterState.UPDATE);
        yield return null;
    }

    private IEnumerator Coroutine_UpdateEncounter()
    {
        SetState(EncounterState.BUILD_QUEUES);
        yield return null;
    }

    //getters/setters

    //character
    public CharacterComponent GetCurrentCharacter()
    {
        return _queues[_currentTeam].Peek();
    }

    public List<CharacterComponent> GetAllCharacters()
    {
        List<CharacterComponent> result = new List<CharacterComponent>();

        foreach (List<CharacterComponent> teamList in _characterMap.Values)
        {
            result.AddRange(teamList);
        }

        return result;
    }

    public List<CharacterComponent> GetAllCharactersInTeam(TeamID teamID)
    {
        return _characterMap[teamID];
    }

    //team queue
    public void PopCurrentCharacter()
    {
        _queues[_currentTeam].Dequeue();
    }

    public bool IsQueueEmpty(TeamID teamID)
    {
        if (_queues.ContainsKey(teamID))
        {
            return (_queues[teamID].Count == 0);
        }

        return false;
    }


    public bool IsCurrentTeamQueueEmpty()
    {
        return IsQueueEmpty(_currentTeam);
    }

    public List<CharacterComponent> GetAllCharactersInTeamQueue(TeamID teamID)
    {
        return new List<CharacterComponent>(_queues[teamID].ToArray());
    }

    //team
    public TeamID GetCurrentTeam()
    {
        return _currentTeam;
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

    //turn count
    public int GetTurnCount()
    {
        return _turnCount;
    }

    public int IncrementTurnCount()
    {
        return _turnCount++;
    }

    public void ToggleCamera(bool flag)
    {
        _virtualCamera.enabled = flag;
    }

    public void SetCameraFollow(Transform target)
    {
        _virtualCamera.Follow = target;
    }

    public void ResetCameraFollow()
    {
        SetCameraFollow(_defaultCameraFollow);
    }
}
