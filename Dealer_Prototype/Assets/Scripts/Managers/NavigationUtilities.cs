using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class NavigationUtilities : MonoBehaviour
{
    private static NavigationUtilities _instance;

    private NavigationUtilities inputActions;

    public static NavigationUtilities Instance { get { return _instance; } }

    private RecastGraph graph;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }

        Build();
    }

    private void Build()
    {
        graph = AstarPath.active.data.recastGraph;
    }

    public bool ValidateDestination(Vector3 destination)
    {
        if (graph != null)
        {
            GraphNode node = graph.PointOnNavmesh(destination, NNConstraint.None);

            if (node != null)
            {
                return true;
            }
        }

        return false;
    }

    public Vector3 GetValidLocation(Vector3 location, out bool valid)
    {
        if (ValidateDestination(location))
        {
            valid = true;
            return graph.GetNearest(location, NNConstraint.Default).clampedPosition;
        }
        else
        {
            if(DebugManager.Instance.LogNavigator) Debug.Log("Invalid destination");
            valid = false;
            return location;
        }
    }
}
