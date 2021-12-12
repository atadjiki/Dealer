using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine;

[DisallowMultipleComponent]
public class PlayerSpawner : MonoBehaviour
{
    public enum SpawnMode { AutoActivate, None };
    public SpawnMode Mode = SpawnMode.AutoActivate;

    public enum PlayerSpawnerState { WaitingToSpawn, Spawning, Spawned };
    private PlayerSpawnerState State = PlayerSpawnerState.WaitingToSpawn;

    private void Awake()
    {
        if (State == PlayerSpawnerState.WaitingToSpawn && Mode == SpawnMode.AutoActivate)
        {
            StartCoroutine(SpawnPlayer());
        }
    }

    public IEnumerator SpawnPlayer()
    {
        State = PlayerSpawnerState.Spawning;

        if (DebugManager.Instance.LogSpawner) Debug.Log("Spawning player");

        GameObject NPC = PrefabFactory.Instance.CreatePrefab(RegistryID.Player, this.transform);
        PlayerComponent playerComp = NPC.GetComponent<PlayerComponent>();

        yield return new WaitWhile(() => playerComp == null);

        playerComp.Initialize(new SpawnData());

        State = PlayerSpawnerState.Spawned;

        yield return null;
    }
}
