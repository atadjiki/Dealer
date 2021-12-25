using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public List<SpawnGroup> SpawnGroups;

    public List<SpawnLocation> SpawnLocations;

    public enum SpawnMode { AutoActivate, None };
    public SpawnMode Mode = SpawnMode.AutoActivate;

    public enum ObjectSpawnerState { WaitingToSpawn, Spawning, Spawned };
    private ObjectSpawnerState State = ObjectSpawnerState.WaitingToSpawn;

    private void Awake()
    {
        if(State == ObjectSpawnerState.WaitingToSpawn && Mode == SpawnMode.AutoActivate)
        {
            ProcessSpawnGroups();
        }
    }

    private void ProcessSpawnGroups()
    {
        foreach (SpawnGroup spawnGroup in SpawnGroups)
        {
            StartCoroutine(ProcessSpawnGroup(spawnGroup));
        }
    }

    private IEnumerator ProcessSpawnGroup(SpawnGroup group)
    {
        yield return new WaitForSeconds(group.Initial_Delay);

        for(int i = 0; i < group.Size; i++)
        {
            if(NPCManager.Instance.HasNotExceededPopCap())
            {
                StartCoroutine(SpawnCharacter(group.Data));
            }

          yield return new WaitForSeconds(group.Delay_Between);

        }

        yield return null;
    }

    public IEnumerator SpawnCharacter(SpawnData data)
    {
        State = ObjectSpawnerState.Spawning;

        DebugManager.Instance.Print(DebugManager.Log.LogSpawner, "Spawning character - " + data.ID.ToString());

        Transform SpawnTransform = this.transform;

        if(SpawnLocations.Count > 0)
        {
            SpawnLocation location = GetLeastUsedLocation();
            
            SpawnTransform = location.transform;

            location.IncrementUses();
        }

        GameObject NPC = PrefabFactory.Instance.CreatePrefab(RegistryID.NPC, SpawnTransform);
        NPCComponent npcComp = NPC.GetComponent<NPCComponent>();

        yield return new WaitWhile(() => npcComp == null);

        npcComp.Initialize(data);

        State = ObjectSpawnerState.Spawned;

        yield return null;
    }

    private SpawnLocation GetLeastUsedLocation()
    {
        SpawnLocation current = SpawnLocations[0];

        for(int i = 1; i < SpawnLocations.Count; i++)
        {
            if(SpawnLocations[i].GetUses() < current.GetUses())
            {
                current = SpawnLocations[i];
            }
        }

        return current ;
    }
}
