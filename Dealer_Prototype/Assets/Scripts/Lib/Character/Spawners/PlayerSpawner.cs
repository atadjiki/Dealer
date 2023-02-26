using System.Collections;
using System.Collections.Generic;
using GameDelegates;
using UnityEngine;

public class PlayerSpawner : Spawner
{
    public void PerformSpawn()
    {
        if (spawnData.SpawnOnClosestPoint)
        {
            bool success;

            Vector3 spawnLocation = NavigationHelper.GetClosestPointOnGraph(this.transform.position, out success);
            if (success)
            {
                this.transform.position = spawnLocation;
            }
        }

        GameObject playerObject = new GameObject("Player " + spawnData.ModelID, new System.Type[] { typeof(PlayerComponent) });
        playerObject.transform.parent = this.transform;
        playerObject.transform.position = this.transform.position;
        playerObject.transform.rotation = this.transform.rotation;

        PlayerComponent playerComponent = playerObject.GetComponent<PlayerComponent>();
        playerComponent.ProcessSpawnData(spawnData);
        playerComponent.Initialize();

        Global.OnPlayerSpawned.Invoke(playerComponent);
    }

    public override string GetSpawning()
    {
        return spawnData.ModelID.ToString();
    }
}
