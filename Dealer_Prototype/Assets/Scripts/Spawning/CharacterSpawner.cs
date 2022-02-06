using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using Constants;
using UnityEngine;

public class CharacterSpawner : MonoBehaviour
{
    public enum SpawnMode { AutoActivate, None };
    public SpawnMode Mode = SpawnMode.AutoActivate;

    public enum CharacterSpawnerState { WaitingToSpawn, Spawning, Spawned };
    protected CharacterSpawnerState State = CharacterSpawnerState.WaitingToSpawn;

    [SerializeField] protected CharacterConstants.CharacterID CharacterID;

    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public virtual IEnumerator PerformSpawn()
    {
        yield return null;
    }

    private void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode)
    {
        if (State == CharacterSpawnerState.WaitingToSpawn && Mode == SpawnMode.AutoActivate)
        {
            StartCoroutine(PerformSpawn());
        }
    }
}
