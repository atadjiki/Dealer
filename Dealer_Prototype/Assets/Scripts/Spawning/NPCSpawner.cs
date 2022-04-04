using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine;

public class NPCSpawner : CharacterSpawner
{
    [SerializeField] private AIConstants.Mode NPC_Mode;

    internal override IEnumerator WaitForManager()
    {
        yield return new WaitUntil(() => NPCManager.Instance != null);

        yield return new WaitUntil(() => NPCManager.Instance.IsInitialized());

        AttemptSpawn();

        yield return null;
    }

    public override void AttemptSpawn()
    {
        base.AttemptSpawn();

        if (State == CharacterSpawnerState.WaitingToSpawn && Mode == SpawnMode.AutoActivate)
        {
            StartCoroutine(PerformSpawn());
        }
    }

    internal override IEnumerator PerformSpawn()
    {
        if(NPCManager.Instance)
        {
            State = CharacterSpawnerState.Spawning;

            DebugManager.Instance.Print(DebugManager.Log.LogSpawner, "Spawning NPC");

            GameObject Character = PrefabFactory.CreatePrefab(RegistryID.NPC, this.transform);
            NPCComponent npcComp = Character.GetComponent<NPCComponent>();

            yield return new WaitWhile(() => npcComp == null);

            spawnData.SetTeam(CharacterConstants.Team.NPC);

            spawnData.SetMode(NPC_Mode);
            spawnData.SetScheduledTasks(ScheduledTasks);

            npcComp.Initialize(spawnData);

            State = CharacterSpawnerState.Spawned;

            yield return new WaitWhile(() => !npcComp.HasInitialized());
        }

        yield return null;
    }
}
