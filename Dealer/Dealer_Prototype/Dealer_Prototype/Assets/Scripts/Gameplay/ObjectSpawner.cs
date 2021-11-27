using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class ObjectSpawner : MonoBehaviour
{
    public List<SpawnGroup> SpawnGroups;

    public List<SpawnLocation> SpawnLocations;

    public enum SpawnMode { AutoActivate, None };
    public SpawnMode Mode = SpawnMode.AutoActivate;

    public enum ObjectSpawnerState { WaitingToSpawn, Spawning, Spawned };
    private ObjectSpawnerState State = ObjectSpawnerState.WaitingToSpawn;

    private Bounds _bounds;

    private void Awake()
    {
        BoxCollider collider = GetComponent<BoxCollider>();
        _bounds = collider.bounds;

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
                StartCoroutine(SpawnCharacter(group.ID, group.BehaviorMode));
            }

          yield return new WaitForSeconds(group.Delay_Between);

        }

        yield return null;
    }

    public IEnumerator SpawnCharacter(CharacterConstants.CharacterID ID, CharacterConstants.Behavior Mode)
    {
        State = ObjectSpawnerState.Spawning;

        if(DebugManager.Instance.LogSpawner) Debug.Log("Spawning character - " + ID.ToString());

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

        npcComp.Initialize(ID, Mode);

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
