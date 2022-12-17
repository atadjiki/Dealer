using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEditor;
using UnityEngine;

public class NPCSpawner : MonoBehaviour
{
    [SerializeField] private CharacterSpawnData data;

    private void Start()
    {
        if(data.SpawnOnClosestPoint)
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
        npcComponent.SetData(data);
        npcComponent.Initialize();
    }


#if UNITY_EDITOR

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(this.transform.position, 0.1f);
        Gizmos.DrawRay(new Ray(this.transform.position, this.transform.forward));

        Handles.Label(this.transform.position + new Vector3(-0.5f, -0.5f, 0), data.ModelID.ToString());
    }
#endif
}
