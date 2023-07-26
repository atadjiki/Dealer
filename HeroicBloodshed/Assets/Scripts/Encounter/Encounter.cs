using System;
using System.Collections;
using System.Collections.Generic;
using Constants;
using Cinemachine;
using UnityEngine;

public class Encounter : MonoBehaviour
{
    private EncounterConstants.State State;

    //keep track of all characters involved in this encounter
    private List<CharacterEncounterData> _PlayerCharacters;
    private List<CharacterEncounterData> _EnemyCharacters;

    //queues are rebuilt each turn based on who is still alive
    private Queue<CharacterEncounterData> _PlayerQueue;
    private Queue<CharacterEncounterData> _EnemyQueue;

    private int _TurnCount;
    private CharacterConstants.TeamID _Turn;

    //setup phase
    [SerializeField] private EncounterSetupData SetupData;

    [SerializeField] private CinemachineVirtualCamera virtualCamera;

    private void Awake()
    {
        State = EncounterConstants.State.NONE;

        _PlayerCharacters = new List<CharacterEncounterData>();
        _EnemyCharacters = new List<CharacterEncounterData>();

        _PlayerQueue = new Queue<CharacterEncounterData>();
        _EnemyQueue = new Queue<CharacterEncounterData>();

        _TurnCount = 0;
        _Turn = CharacterConstants.TeamID.Player;
    }

    public void SpawnCharacters()
    {
        //spawn players
        foreach (PlayerSpawnData spawnData in SetupData.PlayerCharacters)
        {
            GameObject characterObject = CreateCharacterObject("Player_" + spawnData.GetID(), spawnData.Marker.transform);
            PlayerCharacterComponent playerCharacterComponent = characterObject.AddComponent<PlayerCharacterComponent>();
            playerCharacterComponent.PerformSpawn(spawnData.GetID());

            CharacterDefinition def = CharacterDefinition.Get(spawnData.GetID());

            CharacterEncounterData characterData = new CharacterEncounterData(playerCharacterComponent, def.BaseHealth);

            _PlayerCharacters.Add(characterData);
        }

        //spawn enemies
        foreach (EnemySpawnGroupData waveData in SetupData.EnemyGroups)
        {
            foreach (EnemySpawnData spawnData in waveData.Enemies)
            {
                GameObject characterObject = CreateCharacterObject("Enemy" + spawnData.GetID(), spawnData.Marker.transform);
                EnemyCharacterComponent enemyCharacterComponent = characterObject.AddComponent<EnemyCharacterComponent>();
                enemyCharacterComponent.PerformSpawn(spawnData.GetID());

                CharacterDefinition def = CharacterDefinition.Get(spawnData.GetID());

                CharacterEncounterData characterData = new CharacterEncounterData(enemyCharacterComponent, def.BaseHealth);

                _EnemyCharacters.Add(characterData);
            }
        }

        State = EncounterConstants.State.ACTIVE;
    }

    public EncounterConstants.State GetCurrentState()
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

    //helper functions
    private static GameObject CreateCharacterObject(string name, Transform markerTransform)
    {
        GameObject characterObject = new GameObject(name);
        characterObject.transform.parent = markerTransform;
        characterObject.transform.localPosition = Vector3.zero;
        characterObject.transform.localRotation = Quaternion.identity;
        return characterObject;
    }
}
