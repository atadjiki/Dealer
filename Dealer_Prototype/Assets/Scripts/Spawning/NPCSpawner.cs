using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine;

public class NPCSpawner : CharacterSpawner
{
    public override IEnumerator PerformSpawn()
    {
        yield return new WaitForSeconds(2.0f);

        State = CharacterSpawnerState.Spawning;

        DebugManager.Instance.Print(DebugManager.Log.LogSpawner, "Spawning NPC");

        GameObject Character = PrefabFactory.CreatePrefab(Constants.RegistryID.NPC, this.transform);
        NPCComponent npcComp = Character.GetComponent<NPCComponent>();

        yield return new WaitWhile(() => npcComp == null);

        SpawnData npcSpawnData = new SpawnData()
        {
            ID = CharacterID,
            Team = CharacterConstants.Team.NPC
        };

        npcComp.Initialize(npcSpawnData);

        State = CharacterSpawnerState.Spawned;

        yield return new WaitWhile(() => !npcComp.HasInitialized());

        yield return null;
    }
}
