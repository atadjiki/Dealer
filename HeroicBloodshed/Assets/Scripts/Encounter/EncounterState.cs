using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static Constants;

public class EncounterState 
{
    public delegate void EncounterStateChangedDelegate(EncounterStateID stateID);

    public EncounterStateChangedDelegate OnStateChanged;
    
    //collections
    //when setup is performed, store spawned characters
    private Dictionary<TeamID, List<CharacterComponent>> _characterMap;

    //create a queue for each team each combat loop
    private Dictionary<TeamID, Queue<CharacterComponent>> _queues;

    //vars
    private EncounterStateID _stateID;

    private int _turnCount;

    private TeamID _currentTeam;

    //perform setup
    public EncounterState(List<EncounterTeamData> Teams)
    {
       // _stateID = EncounterStateID.NONE;
        _turnCount = 0;
        _currentTeam = TeamID.Player; //the player always goes first

        _characterMap = new Dictionary<TeamID, List<CharacterComponent>>();

        foreach (EncounterTeamData teamData in Teams)
        {
            Debug.Log("Setting up characters for Team: " + teamData.Team);

            _characterMap.Add(teamData.Team, new List<CharacterComponent>());

            //spawn characters for each team 
            foreach (CharacterID characterID in teamData.Characters)
            {
                //see if we have a marker available to spawn them in
                foreach (CharacterMarker marker in teamData.Markers)
                {
                    if (marker.IsOccupied() == false)
                    {
                        Debug.Log("-" + characterID + " at " + marker.gameObject.name);

                        marker.SetOccupied(true);

                        GameObject characterObject = EncounterHelper.CreateCharacterObject(teamData.Team + "_" + characterID, marker.transform);
                        CharacterComponent characterComponent = EncounterHelper.AddComponentByTeam(teamData.Team, characterID, characterObject);

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
            Debug.Log("Setting up queue for Team: " + team);

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

                Debug.Log("Counting " + _queues[team].Count + " for team " + team);
            }
            else
            {
                Debug.Log("Already have an entry for team " + team);
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

        Debug.Log("Current Team: " + _currentTeam);

        return _currentTeam;
    }

    //turn count
    public int GetTurnCount()
    {
        return _turnCount;
    }

    public int IncrementTurnCount()
    {
        Debug.Log("Turn Count: " + _turnCount + 1); return _turnCount++;
    }

    //state ID
    public EncounterStateID CurrentState()
    {
        return _stateID;
    }

    public void TrasitionToState()
    {
        

        if (OnStateChanged != null)
        {
            Debug.Log("Encounter State: " + _stateID); OnStateChanged.Invoke(_stateID);
        }
    }
}
