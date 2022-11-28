using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEditor;
using UnityEngine;

public class NPCSpawner : MonoBehaviour
{
    [SerializeField] private CharacterComponentData data;

    private void Start()
    {
        bool success;

        Vector3 spawnLocation = NavigationHelper.GetClosestPointOnGraph(transform.position, out success);
        if (success)
        {
            GameObject npcObject = new GameObject("NPC " + data.ModelID);
            npcObject.transform.parent = this.transform;
            npcObject.transform.position = spawnLocation;

            NPCComponent npcComponent = npcObject.AddComponent<NPCComponent>();
            npcComponent.Initialize();
        }
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

