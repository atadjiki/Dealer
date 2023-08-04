using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

using static Constants;

[Serializable]
public class Encounter 
{
    public delegate void EncounterStateChangedDelegate(EncounterStateID stateID);

    public EncounterStateChangedDelegate OnStateChanged;
    
    //collections
    //when setup is performed, store spawned characters
    private Dictionary<TeamID, List<CharacterComponent>> _characterMap;

    //create a queue for each team each combat loop
    private Dictionary<TeamID, Queue<CharacterComponent>> _queues;

    //vars
    private EncounterStateID _state;

    private int _turnCount;

    private TeamID _currentTeam;

    private CinemachineVirtualCamera _virtualCamera;

    private Transform _defaultCameraFollow;

    //perform setup
    public Encounter(EncounterSetupData setupData)
    {
        _virtualCamera = setupData.VirtualCamera;
        _defaultCameraFollow = _virtualCamera.Follow;

        // _stateID = EncounterStateID.NONE;
        _turnCount = 0;
        _currentTeam = TeamID.Player; //the player always goes first

        _characterMap = new Dictionary<TeamID, List<CharacterComponent>>();

        foreach (EncounterTeamData teamData in setupData.Teams)
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
    }

    //this is called at the beginning of every turn
    public void BuildTeamQueues()
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
    }

    //check queues and see if we have met the conditions to consider the encounter done
    public void CheckWinConditions()
    {
        //if any queues are empty, then we are done with the encounter
        foreach (Queue<CharacterComponent> TeamQueue in _queues.Values)
        {
            if (TeamQueue.Count == 0)
            {
                //call delegate
                return;
            }
        }
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

        foreach(List<CharacterComponent> teamList in _characterMap.Values)
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

    //state ID
    public EncounterStateID CurrentState()
    {
        return _state;
    }

    public void TrasitionToState()
    {
        if (OnStateChanged != null)
        {
            OnStateChanged.Invoke(_state);
        }
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
