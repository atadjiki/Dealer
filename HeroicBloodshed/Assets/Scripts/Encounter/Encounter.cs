using System;
using System.Collections;
using System.Collections.Generic;

using Cinemachine;
using UnityEngine;

using static Constants;

public class Encounter : MonoBehaviour
{
    [Header("Teams")]
    [SerializeField] private List<EncounterTeamData> Teams;
    [Header("Camera")]
    [SerializeField] private CinemachineVirtualCamera VirtualCamera;

    //when setup is performed, store spawned characters
    private Dictionary<TeamID, List<CharacterComponent>> CharacterMap;

    //create a queue for each team each combat loop
    private Dictionary<TeamID, Queue<CharacterComponent>> TeamQueues;

    private int _turns = 0;

    private TeamID _currentTeam = TeamID.Player; //player starts by default

    private EncounterState _state = EncounterState.NONE;

    private void Awake()
    {
        StartCoroutine(Coroutine_PerformSetup());
    }

    private IEnumerator Coroutine_PerformSetup()
    {
        _state = EncounterState.SETUP;

        yield return PopulateCharacterMap();

        yield return SetupTeamQueues();

        _state = EncounterState.READY;

        ProcessTurn();
    }

    private IEnumerator PopulateCharacterMap()
    {
        CharacterMap = new Dictionary<TeamID, List<CharacterComponent>>();

        foreach (EncounterTeamData teamData in Teams)
        {
            Debug.Log("Setting up characters for Team: " + teamData.Team);

            CharacterMap.Add(teamData.Team, new List<CharacterComponent>());

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

                        GameObject characterObject = CreateCharacterObject(teamData.Team + "_" + characterID, marker.transform);
                        CharacterComponent characterComponent = AddComponentByTeam(teamData.Team, characterID, characterObject);

                        yield return new WaitUntil( () => characterComponent != null );

                        CharacterMap[teamData.Team].Add(characterComponent);

                        break;
                    }


                    Debug.Log("Cannot spawn character, no more markers available");
                }
            }
        }

        yield return null;
    }

    private IEnumerator SetupTeamQueues()
    {
        TeamQueues = new Dictionary<TeamID, Queue<CharacterComponent>>();

        foreach (TeamID team in CharacterMap.Keys)
        {
            Debug.Log("Setting up queue for Team: " + team);

            if (TeamQueues.ContainsKey(team) == false)
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

        yield return null;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) && _state == EncounterState.WAITING_FOR_PLAYER)
        {
            AdvanceTurn();
        }
    }

    public void AdvanceTurn()
    {
        CharacterComponent dequeuedCharacter = TeamQueues[_currentTeam].Dequeue();

        dequeuedCharacter.DestroyDecal();//cleanup character after turn 

        Debug.Log("End turn " + dequeuedCharacter);

        if(ReadyForNextTurn())
        {
            Debug.Log("Advancing to next turn");
            SetupTeamQueues();
            NextTeam();
            IncrementTurns();
            ProcessTurn();
        }
        else
        {
            if (TeamQueues[_currentTeam].Count > 0) //if there are still characters in this team that have yet to go 
            {
                ProcessTurn();
            }
            else //if everyone is done, advance to the next team 
            {
                Debug.Log("next team");
                NextTeam();
                ProcessTurn();
            }
        }
    }

    public void ProcessTurn()
    {
        Debug.Log(_currentTeam + " turn");

        switch (_currentTeam)
        {
            case TeamID.Player:
                ProcessPlayerTurn();
                break;
            case TeamID.Enemy:
                ProcessEnemyTurn();
                break;
        }
    }

    private void ProcessPlayerTurn()
    {
        SelectCharacter(GetCurrentCharacter());

        Debug.Log("Waiting for player action");

        _state = EncounterState.WAITING_FOR_PLAYER;
    }

    private void ProcessEnemyTurn()
    {
        SelectCharacter(GetCurrentCharacter());

        Debug.Log("Performing enemy action");

        _state = EncounterState.BUSY;

        AdvanceTurn();
    }

    public void SelectCharacter(CharacterComponent characterComponent)
    {
        Debug.Log("Selecting character " + characterComponent.GetCharacterID());

        //fix camera on the character
        VirtualCamera.Follow = characterComponent.transform;

        //turn on character decal
        characterComponent.CreateDecal();
    }

    public CharacterComponent GetCurrentCharacter()
    {
        if(TeamQueues.ContainsKey(_currentTeam))
        {
            if(TeamQueues[_currentTeam].Count > 0 )
            {
                return TeamQueues[_currentTeam].Peek();
            }
        }

        return null;
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

    public bool ReadyForNextTurn()
    {
        foreach (TeamID team in TeamQueues.Keys)
        {
            if (TeamQueues[team].Count > 0)
            {
                return false;
            }
        }

        return true;
    }

    public int IncrementTurns()
    {
        Debug.Log("Turn " + _turns + 1);
        return _turns++;
    }

    public TeamID GetCurrentTeam()
    {
        return _currentTeam;
    }

    private void NextTeam()
    {
        switch(_currentTeam)
        {
            case TeamID.Player:
                _currentTeam = TeamID.Enemy;
                break;
            case TeamID.Enemy:
                _currentTeam = TeamID.Player;
                break;
        }
    }
}
