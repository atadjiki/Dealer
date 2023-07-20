using System;
using System.Collections;
using System.Collections.Generic;
using Constants;
using Cinemachine;
using UnityEngine;

public class CombatEncounter : MonoBehaviour
{
    //setup phase
    [SerializeField] private EncounterSetupData encounterData;

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
        foreach (PlayerSpawnData spawnData in encounterData.PlayerCharacters)
        {
            GameObject characterObject = CreateCharacterObject("Player_" + spawnData.GetID(), spawnData.Marker.transform);
            PlayerCharacterComponent playerCharacterComponent = characterObject.AddComponent<PlayerCharacterComponent>();
            playerCharacterComponent.PerformSpawn(spawnData.GetID());
            _playerCharacters.Add(playerCharacterComponent);
        }

        //spawn enemies
        foreach (EnemySpawnGroupData waveData in encounterData.EnemyGroups)
        {
            foreach (EnemySpawnData spawnData in waveData.Enemies)
            {
                GameObject characterObject = CreateCharacterObject("Enemy" + spawnData.GetID(), spawnData.Marker.transform);
                EnemyCharacterComponent enemyCharacterComponent = characterObject.AddComponent<EnemyCharacterComponent>();
                enemyCharacterComponent.PerformSpawn(spawnData.GetID());
                _enemyCharacters.Add(enemyCharacterComponent);
            }
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

    public EncounterSetupData GetEncounterData()
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
