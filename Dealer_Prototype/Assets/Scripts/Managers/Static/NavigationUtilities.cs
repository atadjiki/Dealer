using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class NavigationUtilities : MonoBehaviour
{
    public static bool ValidateDestination(Vector3 destination)
    {
        GridGraph graph = AstarPath.active.data.gridGraph;

        if (graph != null)
        {
            NNInfoInternal info =  graph.GetNearest(destination);

            if (info.node != null)
            {
                return true;
            }
        }

        return false;
    }

    public static Vector3 GetValidLocation(Vector3 location, out bool valid)
    {
        RecastGraph graph = AstarPath.active.data.recastGraph;

        if(graph != null)
        {
            if (ValidateDestination(location))
            {
                valid = true;
                return graph.GetNearest(location, NNConstraint.Default).clampedPosition;
            }
            else
            {
                DebugManager.Instance.Print(DebugManager.Log.LogNavigator, "Invalid destination");
                valid = false;
                return location;
            }
        }

        valid = false;
        return Vector3.zero;
    }
}
