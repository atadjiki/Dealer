using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Constants;

public class SpawnManager : Manager
{
    private static SpawnManager _instance;

    public static SpawnManager Instance { get { return _instance; } }

    private List<SpawnPoint> SpawnPoints;

    public override void Build()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }

        SpawnPoints = new List<SpawnPoint>();

        base.Build();
    }

    public override void Activate()
    {
        base.Activate();
    }

    public override int AssignDelegates()
    {
        GameStateManager.Instance.onGamePlayModeChanged += OnGamePlayModeChanged;

        return 1;
    }

    public void OnGamePlayModeChanged(Constants.State.GamePlayMode GamePlayMode)
    {
        switch(GamePlayMode)
        {
            case State.GamePlayMode.Day:
                DoSpawnUpdate(SpawnPoints[Random.Range(0, SpawnPoints.Count -1)]);
                break;
        }
    }

    public void RegisterSpawnPoint(SpawnPoint spawnPoint)
    {
        if(SpawnPoints.Contains(spawnPoint) == false)
        {
            SpawnPoints.Add(spawnPoint);
          
        }
    }

    public void UnRegisterSpawnPoint(SpawnPoint spawnPoint)
    {
        if(SpawnPoints.Contains(spawnPoint))
        {
            SpawnPoints.Remove(spawnPoint);
        }
    }

    public void DoSpawnUpdate(SpawnPoint spawnPoint)
    {
        //get the party from the game state
        foreach (CharacterInfo characterInfo in GameStateManager.Instance._gameState.partyInfo)
        {
            if (CharacterManager.Instance.IsSpawned(characterInfo) == false)
            {
                AttemptSpawn(spawnPoint, characterInfo);
            }
        }
    }

    public void AttemptSpawn(SpawnPoint spawnPoint, CharacterInfo characterInfo)
    {
        StartCoroutine(PerformSpawn(spawnPoint, characterInfo));
    }

    internal virtual IEnumerator PerformSpawn(SpawnPoint spawnPoint, CharacterInfo characterInfo)
    {
        if (CharacterManager.Instance)
        {
            spawnPoint.SetState(SpawnConstants.CharacterSpawnerState.Spawning);

            DebugManager.Instance.Print(DebugManager.Log.LogSpawner, "Spawning NPC");

            GameObject prefab = PrefabFactory.CreatePrefab(RegistryID.NPC, spawnPoint.transform);
            prefab.name = characterInfo.ID + "-" + characterInfo.name;
            CharacterComponent characterComponent = prefab.GetComponent<CharacterComponent>();

            yield return new WaitWhile(() => characterComponent == null);

            characterComponent.Initialize(characterInfo);

            spawnPoint.SetState(SpawnConstants.CharacterSpawnerState.Spawned);

            yield return new WaitWhile(() => !characterComponent.HasInitialized());
        }

        yield return null;
    }
}
