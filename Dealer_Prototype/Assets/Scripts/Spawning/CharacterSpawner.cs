using System.Collections;
using UnityEngine;

public class CharacterSpawner : MonoBehaviour
{
    public enum SpawnMode { AutoActivate, None };
    public SpawnMode Mode = SpawnMode.AutoActivate;

    public enum CharacterSpawnerState { WaitingToSpawn, Spawning, Spawned };
    protected CharacterSpawnerState State = CharacterSpawnerState.WaitingToSpawn;

    [SerializeField] protected SpawnData spawnData;
    [SerializeField] protected CharacterScheduledTask[] ScheduledTasks;

    private void Awake()
    {
        StartCoroutine(WaitForManager());
    }

    public virtual void AttemptSpawn() { }

    internal virtual IEnumerator WaitForManager()
    {
        yield return null;
    }

    internal virtual IEnumerator PerformSpawn()
    {
        yield return null;
    }
}
