using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Constants;
using UnityEngine;

public class EncounterManager : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera CM_Manager;

    [SerializeField] private Encounter CurrentEncounter;

    private EncounterStateData _stateData;

    private void Awake()
    {
        StartCoroutine(SetupEncounter(CurrentEncounter));
    }

    private IEnumerator SetupEncounter(Encounter encounter)
    {
        Debug.Log("Setting Up Encounter " + encounter.gameObject.name);

        EncounterSetupData setupData = encounter.GetSetupData();

        _stateData = new EncounterStateData();

        SpawnCharacters(setupData);

        yield return new WaitUntil( () => _stateData.IsInitialized() );

        yield return new WaitForSeconds(1.5f);

        CM_Manager.Priority = 0;

        //then switch camera and push UI

        yield return null;
    }

    public void SpawnCharacters(EncounterSetupData setupData)
    { 
        //spawn players
        foreach (PlayerSpawnData spawnData in setupData.PlayerCharacters)
        {
            GameObject characterObject = CreateCharacterObject("Player_" + spawnData.GetID(), spawnData.Marker.transform);
            PlayerCharacterComponent playerCharacterComponent = characterObject.AddComponent<PlayerCharacterComponent>();
            playerCharacterComponent.PerformSpawn(spawnData.GetID());

            CharacterDefinition def = CharacterDefinition.Get(spawnData.GetID());

            CharacterEncounterData characterData = new CharacterEncounterData(playerCharacterComponent, def.BaseHealth);

            _stateData.PlayerCharacters.Add(characterData);
        }

        //spawn enemies
        foreach (EnemySpawnGroupData waveData in setupData.EnemyGroups)
        {
            foreach (EnemySpawnData spawnData in waveData.Enemies)
            {
                GameObject characterObject = CreateCharacterObject("Enemy" + spawnData.GetID(), spawnData.Marker.transform);
                EnemyCharacterComponent enemyCharacterComponent = characterObject.AddComponent<EnemyCharacterComponent>();
                enemyCharacterComponent.PerformSpawn(spawnData.GetID());

                CharacterDefinition def = CharacterDefinition.Get(spawnData.GetID());

                CharacterEncounterData characterData = new CharacterEncounterData(enemyCharacterComponent, def.BaseHealth);

                _stateData.PlayerCharacters.Add(characterData);
            }
        }

        _stateData.Initialized();
    }

    private GameObject CreateCharacterObject(string name, Transform markerTransform)
    {
        GameObject characterObject = new GameObject(name);
        characterObject.transform.parent = markerTransform;
        characterObject.transform.localPosition = Vector3.zero;
        characterObject.transform.localRotation = Quaternion.identity;
        return characterObject;
    }
}
