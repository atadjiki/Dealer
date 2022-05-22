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
        if (BasicCharacterManager.Instance)
        {
            State = CharacterSpawnerState.Spawning;

            DebugManager.Instance.Print(DebugManager.Log.LogSpawner, "Spawning NPC");

            GameObject prefab = new GameObject(characterInfo.ID.ToString());
            prefab.transform.parent = this.transform;
            prefab.transform.position = this.transform.position;
            prefab.transform.rotation = this.transform.rotation;
            BasicCharacter basicCharacter = prefab.AddComponent<BasicCharacter>();

            yield return new WaitWhile(() => basicCharacter == null);

            basicCharacter.Initialize(characterInfo);

            State = CharacterSpawnerState.Spawned;

            yield return new WaitWhile(() => !basicCharacter.HasInitialized());
        }

        yield return null;
    }

}
