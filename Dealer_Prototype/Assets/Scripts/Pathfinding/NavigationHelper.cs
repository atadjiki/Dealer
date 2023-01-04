using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public static class NavigationHelper
{
    public static Vector3 GetClosestPointOnGraph(Vector3 point, out bool success)
    {
        GraphNode node = AstarPath.active.GetNearest(point).node;

        if (node != null && node.Walkable)
        {
            success = true;
            return (Vector3)node.position;
        }
        else
        {
            Debug.Log("Closest Point - failed!");
            success = false;
            return Vector3.zero;
        }
    }

    public static Vector3 GetRandomPointOnGraph(Vector3 origin)
    {
        GraphNode originNode = AstarPath.active.GetNearest(origin).node;
        List<GraphNode> nodes = PathUtilities.GetReachableNodes(originNode);

        GraphNode randomNode = nodes[Random.Range(0,nodes.Count)];

        return randomNode.RandomPointOnSurface();
    }

    public static bool IsPointValid(Vector3 point)
    {
        return AstarPath.active.data.recastGraph.PointOnNavmesh(point, NNConstraint.None) != null; 
    }
}

