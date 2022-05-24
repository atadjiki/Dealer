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

        DoSpawnUpdate();
    }


    public void DoSpawnUpdate()
    {
        //get the party from the game state
        foreach(CharacterInfo characterInfo in GameStateManager.Instance.state.partyInfo)
        {
            if(CharacterManager.Instance.IsSpawned(characterInfo) == false)
            {
                AttemptSpawn(characterInfo);
            }
        }
    }

    public void AttemptSpawn(CharacterInfo characterInfo)
    {
        StartCoroutine(PerformSpawn(characterInfo));
    }

    internal virtual IEnumerator PerformSpawn(CharacterInfo characterInfo)
    {
        if (CharacterManager.Instance)
        {
            State = CharacterSpawnerState.Spawning;

            DebugManager.Instance.Print(DebugManager.Log.LogSpawner, "Spawning NPC");

            GameObject prefab = PrefabFactory.CreatePrefab(RegistryID.NPC, this.transform);
            CharacterComponent characterComponent = prefab.GetComponent<CharacterComponent>();

            yield return new WaitWhile(() => characterComponent == null);

            characterComponent.Initialize(characterInfo);

            State = CharacterSpawnerState.Spawned;

            yield return new WaitWhile(() => !characterComponent.HasInitialized());
        }

        yield return null;
    }

}
