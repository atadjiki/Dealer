using System;
using System.Collections;
using System.Collections.Generic;
using Constants;
using Cinemachine;
using UnityEngine;

public class CombatEncounter : MonoBehaviour
{
    //setup phase
    [SerializeField] private EncounterData encounterData;

    [SerializeField] private CinemachineVirtualCamera encounterCamera;

    private CharacterConstants.TeamID _currentTurn = CharacterConstants.TeamID.Player;

    private List<PlayerCharacterComponent> _playerCharacters;
    private List<EnemyCharacterComponent> _enemyCharacters;

    private void Awake()
    {
        _playerCharacters = new List<PlayerCharacterComponent>();
        _enemyCharacters = new List<EnemyCharacterComponent>();

        SwitchToEncounterCamera();

        Setup();
        Launch();
    }

    public void Setup()
    {
        Debug.Log("Setting Up Encounter " + gameObject.name);

        //spawn players
        foreach (PlayerSpawnData spawnData in encounterData.PlayerSquad)
        {
            GameObject characterObject = new GameObject();
            characterObject.transform.parent = spawnData.Marker.transform;
            PlayerCharacterComponent playerCharacterComponent = characterObject.AddComponent<PlayerCharacterComponent>();
            playerCharacterComponent.PerformSpawn(CharacterConstants.ToCharacterID(spawnData.ID), spawnData.Marker);
            _playerCharacters.Add(playerCharacterComponent);
        }

        //spawn enemies
        foreach (WaveData waveData in encounterData.Waves)
        {
            foreach (EnemySpawnData spawnData in waveData.Enemies)
            {
                GameObject characterObject = new GameObject();
                characterObject.transform.parent = spawnData.Marker.transform;
                EnemyCharacterComponent enemyCharacterComponent = characterObject.AddComponent<EnemyCharacterComponent>();
                enemyCharacterComponent.PerformSpawn(CharacterConstants.ToCharacterID(spawnData.ID), spawnData.Marker);
                _enemyCharacters.Add(enemyCharacterComponent);
            }
        }
    }

    private void Launch()
    {
        ProcessTurn();
    }

    private void ProcessTurn()
    {
        switch (_currentTurn)
        {
            case CharacterConstants.TeamID.Player:
                ProcessPlayerTurn();
                break;
            case CharacterConstants.TeamID.Enemy:
                ProcessEnemyTurn();
                break;
        }
    }

    private void ProcessPlayerTurn()
    {
        Debug.Log("Procesing player turn");

        //iterate through each player character and wait for an ability to be performed 
        foreach(PlayerCharacterComponent playerCharacter in _playerCharacters)
        {
            //select character 
        }
    }

    private void OnPlayerAbilityPerformed()
    {

    }

    private void ProcessEnemyTurn()
    {

    }

    private void OnComplete()
    {
    }

    public void AdvanceTurn()
    {
        if(_currentTurn == CharacterConstants.TeamID.Player)
        {
            _currentTurn = CharacterConstants.TeamID.Enemy;
        }
        else if (_currentTurn == CharacterConstants.TeamID.Enemy)
        {
            _currentTurn = CharacterConstants.TeamID.Player;
        }
    }

    public EncounterData GetEncounterData()
    {
        return encounterData;
    }

    public CinemachineVirtualCamera GetCamera()
    {
        return encounterCamera;
    }

    private void SwitchToOverviewCamera()
    {
        encounterCamera.Priority = 0;

        //CM_Overview.Priority = 10;
    }

    private void SwitchToEncounterCamera()
    {
        //CM_Overview.Priority = 0;

        encounterCamera.Priority = 10;
    }
}
