using System.Collections.Generic;
using Constants;
using UnityEditor;
using UnityEngine;

public class CharacterSpawner : Spawner
{
    [SerializeField] private CharacterSpawnData data;

    public override string GetSpawning()
    {
        return data.ModelID.ToString();
    }

    private void Start()
    {
        if (SpawnOnClosestPoint)
        {
            bool success;

            Vector3 spawnLocation = NavigationHelper.GetClosestPointOnGraph(this.transform.position, out success);
            if (success)
            {
                this.transform.position = spawnLocation;
            }
        }

        GameObject characterObject = new GameObject("Character " + data.ModelID, new System.Type[] { typeof(CharacterComponent) });
        characterObject.transform.parent = this.transform;
        characterObject.transform.position = this.transform.position;
        characterObject.transform.rotation = this.transform.rotation;

        CharacterComponent characterComponent = characterObject.GetComponent<CharacterComponent>();

        characterComponent.ProcessSpawnData(data);
        characterComponent.Initialize();
    }
}
