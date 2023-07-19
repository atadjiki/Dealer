using System;
using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine;

public class CombatEncounter : MonoBehaviour
{
    //setup phase
    [SerializeField] private EncounterData encounterData;
    [SerializeField] private CameraManager CameraManager;

    [SerializeField] private CombatEncounterCamera encounterCamera;

    private void Awake()
    {
        SetupEncounter();
    }

    public void SetupEncounter()
    {
        StartCoroutine(Coroutine_SetupEncounter());
    }

    private IEnumerator Coroutine_SetupEncounter()
    {
        //spawn players
        foreach (PlayerSpawnData spawnData in encounterData.PlayerSquad)
        {
            GameObject characterObject = new GameObject();
            characterObject.transform.parent = spawnData.Marker.transform;
            PlayerCharacterComponent characterComponent = characterObject.AddComponent<PlayerCharacterComponent>();
            characterComponent.PerformSpawn(CharacterConstants.ToCharacterID(spawnData.ID), spawnData.Marker);
        }

        //spawn enemies
        foreach (WaveData waveData in encounterData.Waves)
        {
            foreach (EnemySpawnData spawnData in waveData.Enemies)
            {
                GameObject characterObject = new GameObject();
                characterObject.transform.parent = spawnData.Marker.transform;
                EnemyCharacterComponent characterComponent = characterObject.AddComponent<EnemyCharacterComponent>();
                characterComponent.PerformSpawn(CharacterConstants.ToCharacterID(spawnData.ID), spawnData.Marker);
            }
        }

        yield return null;
    }
}
