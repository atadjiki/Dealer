using System;
using System.Collections;
using System.Collections.Generic;
using static Constants;
using Cinemachine;
using UnityEngine;

public class Encounter : MonoBehaviour
{
    //setup data
    [SerializeField] private List<EncounterTeamData> Teams;

    //when setup is performed, store spawned characters
    private Dictionary<TeamID, List<CharacterComponent>> CharacterMap;

    //create a queue for each team each combat loop
    private Dictionary<TeamID, Queue<CharacterComponent>> TeamQueues;

    private TeamID _currentTurn = TeamID.Player; //player starts by default 

    private void Awake()
    {
        PopulateCharacterMap(); //generate characters based on spawn data

        SetupTeamQueues(); //populate team queues based on who is alive, etc

        ProcessEncounterLoop();
    }

    public void ProcessEncounterLoop()
    {
        Debug.Log(_currentTurn + " turn");

        switch (_currentTurn)
        {
            case TeamID.Player:
                ProcessPlayerTurn();
                break;
            case TeamID.Enemy:
                break;
        }
    }

    private void ProcessPlayerTurn()
    {
        SelectCharacter(GetCurrentCharacter());

        Debug.Log("Waiting for player action");
    }

    public void SelectCharacter(CharacterComponent characterComponent)
    {
        Debug.Log("Selecting character " + characterComponent.GetCharacterID());
    }

    public CharacterComponent GetCurrentCharacter()
    {
        if(TeamQueues.ContainsKey(_currentTurn))
        {
            return TeamQueues[_currentTurn].Peek();
        }

        return null;
    }

    public void SetupTeamQueues()
    {
        TeamQueues = new Dictionary<TeamID, Queue<CharacterComponent>>();

        foreach(TeamID team in CharacterMap.Keys)
        {
            Debug.Log("Setting up queue for Team: " + team);

            if(TeamQueues.ContainsKey(team) == false)
            {
                TeamQueues.Add(team, new Queue<CharacterComponent>()); //create a new queue

                foreach (CharacterComponent characterComponent in CharacterMap[team])
                {
                    if (characterComponent.IsAlive())
                    {
                        TeamQueues[team].Enqueue(characterComponent);
                    }
                }

                Debug.Log("Counting " + TeamQueues[team].Count + " for team " + team);
            }
            else
            {
                Debug.Log("Already have an entry for team " + team);
            }
        }
    }

    public void PopulateCharacterMap()
    {
        CharacterMap = new Dictionary<TeamID, List<CharacterComponent>>();

        foreach(EncounterTeamData teamData in Teams)
        {
            Debug.Log("Setting up characters for Team: " + teamData.Team);

            CharacterMap.Add(teamData.Team, new List<CharacterComponent>());

            //spawn characters for each team 
            foreach (CharacterID characterID in teamData.Characters)
            {
                //see if we have a marker available to spawn them in
                foreach (CharacterMarker marker in teamData.Markers)
                {
                    if(marker.IsOccupied() == false)
                    {
                        Debug.Log("-" + characterID + " at " + marker.gameObject.name);

                        marker.SetOccupied(true);

                        GameObject characterObject = CreateCharacterObject(teamData.Team + "_" + characterID, marker.transform);
                        CharacterComponent characterComponent = AddComponentByTeam(teamData.Team, characterID, characterObject);

                        CharacterMap[teamData.Team].Add(characterComponent);

                        break;
                    }


                    Debug.Log("Cannot spawn character, no more markers available");
                }
            }
        }
    }

    private CharacterComponent AddComponentByTeam(TeamID teamID, CharacterID characterID, GameObject characterObject)
    {
        switch(teamID)
        {
            case TeamID.Player:
                PlayerCharacterComponent playerCharacterComponent = characterObject.AddComponent<PlayerCharacterComponent>();
                playerCharacterComponent.PerformSetup(characterID);
                return playerCharacterComponent;
            case TeamID.Enemy:
                EnemyCharacterComponent enemyCharacterComponent = characterObject.AddComponent<EnemyCharacterComponent>();
                enemyCharacterComponent.PerformSetup(characterID);
                return enemyCharacterComponent;
            default:
                return null;
        }
    }

    private GameObject CreateCharacterObject(string name, Transform markerTransform)
    {
        GameObject characterObject = new GameObject(name);
        characterObject.transform.parent = markerTransform;
        characterObject.transform.localPosition = Vector3.zero;
        characterObject.transform.localRotation = Quaternion.identity;
        return characterObject;
    }

    public TeamID GetCurrentTurn()
    {
        return _currentTurn;
    }

    private void IncrementTurn()
    {
        switch(_currentTurn)
        {
            case TeamID.Player:
                _currentTurn = TeamID.Enemy;
                break;
            case TeamID.Enemy:
                _currentTurn = TeamID.Player;
                break;
        }
    }
}
