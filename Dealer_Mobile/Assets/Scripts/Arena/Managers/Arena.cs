using System;
using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine;

public class Arena : MonoBehaviour
{
    //setup phase
    [SerializeField] private EncounterData data;
    [SerializeField] private CameraManager CameraManager;

    [SerializeField] private ArenaCamera ArenaCamera;

    private void Awake()
    {
        Launch();
    }

    public void Launch()
    {
        StartCoroutine(Coroutine_SetupArena());
    }

    private IEnumerator Coroutine_SetupArena()
    {
        foreach (WaveData teamData in data.Waves)
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
