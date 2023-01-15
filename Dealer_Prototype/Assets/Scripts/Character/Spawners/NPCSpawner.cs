using UnityEngine;

public class NPCSpawner : Spawner
{
    private void Start()
    {
        if(spawnData.ModelID == Constants.Enumerations.CharacterModelID.None) { return; }

        if(spawnData.SpawnOnClosestPoint)
        {
            bool success;

            Vector3 spawnLocation = NavigationHelper.GetClosestPointOnGraph(this.transform.position, out success);
            if (success)
            {
                this.transform.position = spawnLocation;
            }
        }

        GameObject npcObject = new GameObject("NPC " + spawnData.ModelID, new System.Type[] { typeof(NPCComponent) });
        npcObject.transform.parent = this.transform;
        npcObject.transform.position = this.transform.position;
        npcObject.transform.rotation = this.transform.rotation;

        NPCComponent npcComponent = npcObject.GetComponent<NPCComponent>();

        npcComponent.ProcessSpawnData(spawnData);
        npcComponent.Initialize();
       // npcComponent.StartNPC();
    }

    public override string GetSpawning()
    {
        return spawnData.ModelID.ToString();
    }
}
