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
        foreach (WaveData teamData in encounterData.Waves)
        {
            foreach (CharacterSpawnData spawnData in teamData.Characters)
            {
                GameObject characterObject = new GameObject();
                characterObject.transform.parent = spawnData.Marker.transform;
                CharacterComponent characterComponent = characterObject.AddComponent<CharacterComponent>();
                characterComponent.PerformSpawn(spawnData);
            }
        }

        yield return null;
    }
}
