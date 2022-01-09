using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine.SceneManagement;
using UnityEngine;

[DisallowMultipleComponent]
public class PlayableCharacterSpawner : MonoBehaviour
{
    public enum SpawnMode { AutoActivate, None };
    public SpawnMode Mode = SpawnMode.AutoActivate;

    public enum PlayerSpawnerState { WaitingToSpawn, Spawning, Spawned };
    private PlayerSpawnerState State = PlayerSpawnerState.WaitingToSpawn;

    [SerializeField] private CharacterConstants.CharacterID CharacterID;

    [SerializeField] private bool PlayerLock = true;

    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode)
    {
        if (State == PlayerSpawnerState.WaitingToSpawn && Mode == SpawnMode.AutoActivate)
        {
            StartCoroutine(SpawnPlayer());
        }
    }

    public IEnumerator SpawnPlayer()
    {
        yield return new WaitForSeconds(2.0f);

        State = PlayerSpawnerState.Spawning;

        DebugManager.Instance.Print(DebugManager.Log.LogSpawner, "Spawning playable character");

        GameObject Character = PrefabFactory.CreatePrefab(RegistryID.Player, this.transform);
        PlayableCharacterComponent playerComp = Character.GetComponent<PlayableCharacterComponent>();

        yield return new WaitWhile(() => playerComp == null);

        SpawnData playerSpawnData = new SpawnData()
        {
            ID = CharacterID,
            Team = CharacterConstants.Team.Ally
        };


        playerComp.Initialize(playerSpawnData);

        State = PlayerSpawnerState.Spawned;

        yield return new WaitForSeconds(0.25f);

        if (PlayerLock)
        {
            PlayableCharacterManager.Instance.LockToCharacter(playerComp);
        }

        yield return null;
    }
}
