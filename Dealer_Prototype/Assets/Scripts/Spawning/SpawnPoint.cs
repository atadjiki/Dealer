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

        if (CharacterManager.Instance)
        {
            State = CharacterSpawnerState.Spawning;

            DebugManager.Instance.Print(DebugManager.Log.LogSpawner, "Spawning NPC");

            GameObject prefab = PrefabFactory.GetCharacterPrefab(characterInfo.ID.ToString(), this.transform);
            CharacterComponent characterModel = prefab.GetComponent<CharacterComponent>();

            yield return new WaitWhile(() => characterModel == null);

            characterModel.Initialize(characterInfo);

            State = CharacterSpawnerState.Spawned;

            yield return new WaitWhile(() => !characterModel.HasInitialized());
        }

        yield return null;
    }

}
