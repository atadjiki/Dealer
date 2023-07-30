using System;
using System.Collections;
using System.Collections.Generic;

using Cinemachine;
using UnityEngine;
using static Constants;

public class EncounterState
{
    private EncounterStateID _stateID;

    private int _turnCount;

    private TeamID _currentTeam;

    public EncounterState()
    {
        _stateID = EncounterStateID.NONE;
        _turnCount = 0;
        _currentTeam = TeamID.Player; //the player always goes first 
    }

    public void To(EncounterStateID ID)
    {
        _stateID = ID;
        Debug.Log("Encounter State: " + _stateID);
    }
    public EncounterStateID Get() { return _stateID; }

    public TeamID IncrementTeam()
    {
        switch(_currentTeam)
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

    public TeamID GetCurrentTeam() { return _currentTeam; }

    public int IncrementTurnCount()
    {
        _turnCount += 1;
        Debug.Log("Turn Count: " + _turnCount);
        return _turnCount;
    }

    public int GetTurnCount() { return _turnCount; }
}

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

    private EncounterState _state;

    private void Awake()
    {
        _state = new EncounterState();

        Setup();
    }

    //Public

    //process spawn data and setup characters before we start the encounter 
    public void Setup()
    {
        StartCoroutine(Coroutine_PerformSetup());
    }

    //this is called at the beginning of every turn
    public void BuildTeamQueues()
    {
        StartCoroutine(Coroutine_BuildTeamQueues());
    }

    //check queues and see if we have met the conditions to consider the encounter done
    public void CheckWinConditions()
    {
        //if any queues are empty, then we are done with the encounter
        foreach (Queue<CharacterComponent> TeamQueue in TeamQueues.Values)
        {
            if(TeamQueue.Count == 0)
            {
                //call delegate
                OnEncounterComplete();
                return;
            }
        }

        PrepareTurn();
    }

    //focus on the character who is currently performing an ability
    public void PrepareTurn()
    {
        CharacterComponent currentCharacter = TeamQueues[_state.GetCurrentTeam()].Peek();

        //create decal
        currentCharacter.CreateDecal();

        //focus camera on character
        VirtualCamera.Follow = currentCharacter.transform;

        //show appropriate UI

        ProcessTurn();
    }

    //allow every team to perform an ability, then build the queues again 
    public void ProcessTurn()
    {
        //foreach team
        //foreach character in team

        TeamID CurrentTeam = _state.GetCurrentTeam();

        if(CurrentTeam == TeamID.Player)
        {
            ChooseCharacterAbility();
            return;
        }
        else if(CurrentTeam == TeamID.Enemy)
        {
            ChooseCharacterAbility();
            return;
        }
    }

    //wait for player to choose an ability to perform 
    public void WaitOnPlayerInput()
    {
        //hold until we receive input from player
        _state.To(EncounterStateID.WAITING_ON_INPUT);
    }

    //non player AI will use this to determine their action for the turn 
    public void ChooseCharacterAbility()
    {
        //choose ability on character stats/abilities, etc
        _state.To(EncounterStateID.WAITING_ON_AI);

        CharacterComponent currentCharacter = TeamQueues[_state.GetCurrentTeam()].Peek();

        AbilityID chosenAbility = GetAllowedAbilities(currentCharacter.GetCharacterID())[0];

        PerformCharacterAbility(chosenAbility);
    }

    //perform ability for the currently selected character
    public void PerformCharacterAbility(AbilityID abilityID)
    {
        //block input, play animation, do effects, update character data to reflect, update UI
        switch(abilityID)
        {
            default:
                Debug.Log("Performing ability " + abilityID);
                StartCoroutine(Coroutine_PerformAbility_SkipTurn());
                break;
        }
    }

    private IEnumerator Coroutine_PerformAbility_SkipTurn()
    {
        yield return new WaitForSeconds(2.5f);

        CleanupTurn();
    }

    //deselect current character before starting a new turn 
    public void CleanupTurn()
    {
        //deselect the current character 
        CharacterComponent currentCharacter = TeamQueues[_state.GetCurrentTeam()].Peek();

        //destroy any decals
        currentCharacter.DestroyDecal();

        //recenter camera
        VirtualCamera.Follow = null;

        //pop the current character from the queue 
        TeamQueues[_state.GetCurrentTeam()].Dequeue();

        //update the encounter state 
        UpdateState();
    }

    public void UpdateState()
    {
        _state.IncrementTurnCount();

        //check if we are done with this team 
        if (TeamQueues[_state.GetCurrentTeam()].Count == 0)
        {
            Debug.Log(_state.GetCurrentTeam() + " team done");

            if(_state.IncrementTeam() == TeamID.None)
            {
                //if all teams have gone, then time to rebuild queues
                BuildTeamQueues();
                return;
            }
        }

        PrepareTurn();
    }

    //Callbacks

    private void OnSetupComplete()
    {
        Debug.Log("Setup compete");
        _state.To(EncounterStateID.BUSY);

        BuildTeamQueues();
    }

    private void OnQueuesBuilt()
    {
        Debug.Log("Team Queues Built");
        _state.To(EncounterStateID.BUSY);

        CheckWinConditions();
    }

    private void OnEncounterComplete()
    {
        Debug.Log("Encounter Complete");
        _state.To(EncounterStateID.DONE);
    }

    //Coroutines
    private IEnumerator Coroutine_PerformSetup()
    {
        _state.To(EncounterStateID.SETUP);

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

                        yield return new WaitUntil(() => characterComponent != null);

                        CharacterMap[teamData.Team].Add(characterComponent);

                        break;
                    }
                }
            }
        }

        yield return new WaitForSeconds(1.0f);

        OnSetupComplete();
        yield return null;
    }

    private IEnumerator Coroutine_BuildTeamQueues()
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

        OnQueuesBuilt();
        yield return null;
    }

    //Helpers

    private static CharacterComponent AddComponentByTeam(TeamID teamID, CharacterID characterID, GameObject characterObject)
    {
        switch (teamID)
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

    private static GameObject CreateCharacterObject(string name, Transform markerTransform)
    {
        GameObject characterObject = new GameObject(name);
        characterObject.transform.parent = markerTransform;
        characterObject.transform.localPosition = Vector3.zero;
        characterObject.transform.localRotation = Quaternion.identity;
        return characterObject;
    }
}
