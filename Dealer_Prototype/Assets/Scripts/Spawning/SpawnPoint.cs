using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public enum CharacterSpawnerState { WaitingToSpawn, Spawning, Spawned };
    protected CharacterSpawnerState State = CharacterSpawnerState.WaitingToSpawn;

    private static SpawnPoint _instance;

    public static SpawnPoint Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    public void AttemptSpawn(CharacterInfo characterInfo)
    {
        StartCoroutine(PerformSpawn(characterInfo));
    }

    internal virtual IEnumerator PerformSpawn(CharacterInfo characterInfo)
    {
        if (NPCManager.Instance)
        {
            State = CharacterSpawnerState.Spawning;

            DebugManager.Instance.Print(DebugManager.Log.LogSpawner, "Spawning NPC");

            GameObject Character = PrefabFactory.CreatePrefab(RegistryID.NPC, this.transform);
            NPCComponent npcComp = Character.GetComponent<NPCComponent>();

            yield return new WaitWhile(() => npcComp == null);

            SpawnData spawnData = new SpawnData();
            spawnData.ID = characterInfo.ID;
            spawnData.InitialAnim = characterInfo.InitialAnim;
            spawnData.SetMode(AIConstants.Mode.Stationary);
            spawnData.SetTeam(CharacterConstants.Team.Ally);

            npcComp.Initialize(spawnData);

            State = CharacterSpawnerState.Spawned;

            yield return new WaitWhile(() => !npcComp.HasInitialized());
        }

        yield return null;
    }

}
