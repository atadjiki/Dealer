using System;
using System.Collections;
using System.Collections.Generic;
using static Constants;
using Cinemachine;
using UnityEngine;

public class Encounter : MonoBehaviour
{
    private EncounterState State;

    //keep track of all characters involved in this encounter
    private Dictionary<TeamID, List<CharacterEncounterData>> CharacterMap;

    private int _TurnCount;
    private TeamID _Turn;

    //setup phase
    [SerializeField] private EncounterSetupData SetupData;

    [SerializeField] private CinemachineVirtualCamera virtualCamera;

    private void Awake()
    {
        State = EncounterState.NONE;

        InitCharacterMap();
            
        _TurnCount = 0;
        _Turn = TeamID.Player;
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

            AddCharacterToList(TeamID.Player, characterData);
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

                AddCharacterToList(TeamID.Enemy, characterData);
            }
        }

        State = EncounterState.ACTIVE;
    }

    public EncounterState GetCurrentState()
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
        CharacterMap = new Dictionary<TeamID, List<CharacterEncounterData>>();

        //create a mapping for each team 
        foreach (TeamID Team in Enum.GetValues(typeof(TeamID)))
        {
            CharacterMap.Add(Team, new List<CharacterEncounterData>());
        }
    }

    private void AddCharacterToList(TeamID Team, CharacterEncounterData Data)
    {
        if(CharacterMap.ContainsKey(Team))
        {
            CharacterMap[Team].Add(Data);
        }
    }
}
