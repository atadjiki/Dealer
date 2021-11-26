using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class ObjectSpawner : MonoBehaviour
{
    public List<SpawnGroup> SpawnGroups;

    public enum ObjectSpawnerState { WaitingToSpawn, Spawning, Spawned };
    private ObjectSpawnerState State = ObjectSpawnerState.WaitingToSpawn;

    private Bounds _bounds;

    private void Awake()
    {
        BoxCollider collider = GetComponent<BoxCollider>();
        _bounds = collider.bounds;

        if(State == ObjectSpawnerState.WaitingToSpawn)
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
            StartCoroutine(SpawnCharacter(group.ID));

          yield return new WaitForSeconds(group.Delay_Between);
        }

        yield return null;
    }

    public IEnumerator SpawnCharacter(CharacterConstants.CharacterID ID)
    {
        State = ObjectSpawnerState.Spawning;

        Debug.Log("Spawning character - " + ID.ToString());

        Vector3 RandomLocation;

        RandomLocation.x = Random.Range(_bounds.min.x, _bounds.max.x);
        RandomLocation.y = this.transform.position.y;
        RandomLocation.z = Random.Range(_bounds.min.z, _bounds.max.z);

        float RandomRotation = Random.Range(0, 360);

        GameObject NPC = PrefabFactory.Instance.CreatePrefab(RegistryID.NPC, this.transform);
        NPCComponent npcComp = NPC.GetComponent<NPCComponent>();

        yield return new WaitWhile(() => npcComp == null);

        if(npcComp != null)
        {
            npcComp.CharacterID = ID;
            npcComp.Initialize();
          //  npcComp.SetPositionRotation(RandomLocation, Quaternion.Euler(0, RandomRotation, 0));
        }

        State = ObjectSpawnerState.Spawned;

        yield return null;
    }
}
