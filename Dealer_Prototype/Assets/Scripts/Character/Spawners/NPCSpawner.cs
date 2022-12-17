using UnityEngine;

public class NPCSpawner : Spawner
{
    [SerializeField] protected NPCSpawnData data;

    private void Start()
    {
        if(SpawnOnClosestPoint)
        {
            bool success;

            Vector3 spawnLocation = NavigationHelper.GetClosestPointOnGraph(this.transform.position, out success);
            if (success)
            {
                this.transform.position = spawnLocation;
            }
        }

        GameObject npcObject = new GameObject("NPC " + data.ModelID, new System.Type[] { typeof(NPCComponent) });
        npcObject.transform.parent = this.transform;
        npcObject.transform.position = this.transform.position;
        npcObject.transform.rotation = this.transform.rotation;

        NPCComponent npcComponent = npcObject.GetComponent<NPCComponent>();

        npcComponent.ProcessSpawnData(data);
        npcComponent.Initialize();
       // npcComponent.StartNPC();
    }

    public override string GetSpawning()
    {
        return data.ModelID.ToString();
    }
}
