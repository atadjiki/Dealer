using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

//Interface for State Graphs (visual scripting) to use 
public class EncounterUtil : MonoBehaviour
{
    public static TileGraph GetEnvironmentGraph()
    {
        TileGraph graph = (TileGraph)AstarPath.active.data.FindGraphOfType(typeof(TileGraph));

        return graph;
    }

    public static Vector3 GetRandomTile()
    {
        TileGraph graph = GetEnvironmentGraph();

        PointNode node = graph.GetRandomNode();

        if(node != null)
        {
            return (Vector3)node.position;
        }
        else
        {
            Debug.LogError("Random node was null!");
            return Vector3.zero;
        }
    }
}
