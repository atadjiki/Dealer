using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;
using UnityEngine.EventSystems;
using static Constants;

public class EnvironmentUtil : MonoBehaviour
{

    public static bool GetClosestNodeInArea(Vector3 origin, Vector3 size, out Vector3 result)
    {
        GridGraph gridGraph = AstarPath.active.data.gridGraph;

        Bounds bounds = new Bounds(origin, size);

        Dictionary<float, GraphNode> NodesInProximity = new Dictionary<float, GraphNode>();

        foreach(GraphNode node in gridGraph.GetNodesInRegion(bounds))
        {
            if(node.Walkable)
            {
                float distance = Vector3.Distance(origin, (Vector3)node.position);

                if (!NodesInProximity.ContainsKey(distance))
                {
                    NodesInProximity.Add(distance, node);
                }
            }
        }

        if(NodesInProximity.Count > 0)
        {
            Debug.Log("Found " + NodesInProximity.Count + " nodes in area");

            KeyValuePair<float, GraphNode> closestNode = new KeyValuePair<float, GraphNode>();

            foreach(KeyValuePair<float, GraphNode> pair in NodesInProximity)
            {
                closestNode = pair;
                break;
            }

            result = (Vector3)closestNode.Value.position;
            return true;
        }

        result = Vector3.zero;
        return false;
    }

    public static List<Vector3> GetNeighboringNodes(Bounds bounds)
    {
        List<Vector3> neighbors = new List<Vector3>();

        foreach(Vector3 nodePosition in GetNodesInBounds(bounds))
        {
            foreach(Vector3 neighbor in GetNeighboringNodes(nodePosition))
            {
                if(!neighbors.Contains(neighbor))
                {
                    neighbors.Add(neighbor);
                }
            }
        }

        return neighbors;
    }

    public static List<Vector3> GetNeighboringNodes(Vector3 position)
    {
        GridGraph gridGraph = AstarPath.active.data.gridGraph;

        NNInfoInternal nnInfo = gridGraph.GetNearest(position, BuildConstraint());

        List<Vector3> connections = new List<Vector3>();

        if (nnInfo.node != null)
        {
            nnInfo.node.GetConnections(otherNode =>
            {
                connections.Add((Vector3) otherNode.position);
            });
        }

        return connections;
    }

    public static bool GetClosestNodeToPosition(Vector3 position, out Vector3 result)
    {
        GridGraph gridGraph = AstarPath.active.data.gridGraph;

        NNInfoInternal nnInfo = gridGraph.GetNearest(position, BuildConstraint());

        if (nnInfo.node != null)
        {
            result = (Vector3)nnInfo.node.position;
            return true;
        }
        else
        {
            result = Vector3.zero;
            return false;
        }
    }

    public static List<Vector3> GetNodesInBounds(Bounds bounds)
    {
        List<Vector3> nodes = new List<Vector3>();

        GridGraph gridGraph = AstarPath.active.data.gridGraph;

        foreach(GraphNode graphNode in gridGraph.GetNodesInRegion(bounds))
        {
            nodes.Add((Vector3)graphNode.position);
        }

        return nodes;
    }

    public static List<GraphNode> GetGraphNodesInBounds(Bounds bounds)
    {
        GridGraph gridGraph = AstarPath.active.data.gridGraph;

        return gridGraph.GetNodesInRegion(bounds);
    }

    public static NNConstraint BuildConstraint()
    {
        NNConstraint constraint = new NNConstraint();
        constraint.constrainWalkability = true;
        constraint.constrainDistance = true;
        constraint.walkable = true;
        return constraint;
    }

    public static CharacterComponent AddComponentByTeam(CharacterID characterID, GameObject characterObject)
    {
        TeamID teamID = GetTeamByID(characterID);

        switch (teamID)
        {
            case TeamID.Player:
                PlayerCharacterComponent playerCharacterComponent = characterObject.AddComponent<PlayerCharacterComponent>();
                playerCharacterComponent.SetID(characterID);
                return playerCharacterComponent;
            case TeamID.Enemy:
                EnemyCharacterComponent enemyCharacterComponent = characterObject.AddComponent<EnemyCharacterComponent>();
                enemyCharacterComponent.SetID(characterID);
                return enemyCharacterComponent;
            default:
                return null;
        }
    }

    public static bool IsWithinCharacterRange(int distance, CharacterComponent character, out MovementRangeType rangeType)
    {
        int threshold;

        int AP = character.GetActionPoints();
        int movementRange = character.GetMovementRange();

        if (AP >= GetAbilityCost(AbilityID.MoveFull))
        {
            threshold = (movementRange * 2);
        }
        else
        {
            threshold = movementRange;
        }

        if (distance <= threshold)
        {
            if (distance <= character.GetMovementRange())
            {
                rangeType = MovementRangeType.Half;
            }
            else if(AP > 1)
            {
                rangeType = MovementRangeType.Full;
            }
            else
            {
                rangeType = MovementRangeType.Half;
            }

            return true;
        }

        rangeType = MovementRangeType.None;
        return false;
    }

    public static bool CheckIsMouseBlocked()
    {
        if (Camera.main == null)
        {
            return true;
        }

        if (EventSystem.current.IsPointerOverGameObject())
        {
            return true;
        }

        return false;
    }
}
