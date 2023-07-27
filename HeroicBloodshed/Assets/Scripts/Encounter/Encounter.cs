using System;
using System.Collections;
using System.Collections.Generic;
using Constants;
using Cinemachine;
using UnityEngine;

public class Encounter : MonoBehaviour
{
    private Constants.Encounter.State State;

    //keep track of all characters involved in this encounter
    private Dictionary<Game.TeamID, List<CharacterEncounterData>> CharacterMap;

    private int _TurnCount;
    private Game.TeamID _Turn;

    //setup phase
    [SerializeField] private EncounterSetupData SetupData;

    [SerializeField] private CinemachineVirtualCamera virtualCamera;

    private void Awake()
    {
        State = Constants.Encounter.State.NONE;

        InitCharacterMap();
            
        _TurnCount = 0;
        _Turn = Game.TeamID.Player;
    }

    public void SpawnCharacters()
    {
        //spawn players
        foreach (PlayerSpawnData spawnData in SetupData.PlayerCharacters)
        {
            GameObject characterObject = EncounterHelper.CreateCharacterObject("Player_" + spawnData.GetID(), spawnData.Marker.transform);
            PlayerCharacterComponent playerCharacterComponent = characterObject.AddComponent<PlayerCharacterComponent>();
            playerCharacterComponent.PerformSpawn(spawnData.GetID());

            CharacterDefinition def = CharacterDefinition.Get(spawnData.GetID());

            CharacterEncounterData characterData = new CharacterEncounterData(playerCharacterComponent, def.BaseHealth);

            AddCharacterToList(Game.TeamID.Player, characterData);
        }

        //spawn enemies
        foreach (EnemySpawnGroupData waveData in SetupData.EnemyGroups)
        {
            foreach (EnemySpawnData spawnData in waveData.Enemies)
            {
                GameObject characterObject = EncounterHelper.CreateCharacterObject("Enemy" + spawnData.GetID(), spawnData.Marker.transform);
                EnemyCharacterComponent enemyCharacterComponent = characterObject.AddComponent<EnemyCharacterComponent>();
                enemyCharacterComponent.PerformSpawn(spawnData.GetID());

                CharacterDefinition def = CharacterDefinition.Get(spawnData.GetID());

                CharacterEncounterData characterData = new CharacterEncounterData(enemyCharacterComponent, def.BaseHealth);

                AddCharacterToList(Game.TeamID.Enemy, characterData);
            }
        }

        State = Constants.Encounter.State.ACTIVE;
    }

    public Constants.Encounter.State GetCurrentState()
    {
        return State;
    }

    public EncounterSetupData GetSetupData()
    {
        return SetupData;
    }

    public CinemachineVirtualCamera GetCamera()
    {
        return virtualCamera;
    }

    //private
    private void InitCharacterMap()
    {
        CharacterMap = new Dictionary<Game.TeamID, List<CharacterEncounterData>>();

        //create a mapping for each team 
        foreach (Game.TeamID Team in Enum.GetValues(typeof(Game.TeamID)))
        {
            CharacterMap.Add(Team, new List<CharacterEncounterData>());
        }
    }

    private void AddCharacterToList(Game.TeamID Team, CharacterEncounterData Data)
    {
        if(CharacterMap.ContainsKey(Team))
        {
            CharacterMap[Team].Add(Data);
        }
    }
}
