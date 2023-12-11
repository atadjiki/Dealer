using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;
using UnityEngine.EventSystems;
using static Constants;

public class EnvironmentUtil : MonoBehaviour
{
    public static bool GetClosestGraphNodeInArea(Vector3 origin, Vector3 size, out GraphNode result)
    {
        GridGraph gridGraph = AstarPath.active.data.gridGraph;

        Bounds bounds = new Bounds(origin, size);

        Dictionary<float, GraphNode> NodesInProximity = new Dictionary<float, GraphNode>();

        foreach (GraphNode node in gridGraph.GetNodesInRegion(bounds))
        {
            if (node.Walkable)
            {
                float distance = Vector3.Distance(origin, (Vector3)node.position);

                if (!NodesInProximity.ContainsKey(distance))
                {
                    NodesInProximity.Add(distance, node);
                }
            }
        }

        if (NodesInProximity.Count > 0)
        {
            Debug.Log("Found " + NodesInProximity.Count + " nodes in area");

            KeyValuePair<float, GraphNode> closestNode = new KeyValuePair<float, GraphNode>();

            foreach (KeyValuePair<float, GraphNode> pair in NodesInProximity)
            {
                closestNode = pair;
                break;
            }

            result = closestNode.Value;
            return true;
        }

        result = null;
        return false;
    }

    public static List<GraphNode> GetGraphNodesInBounds(Bounds bounds)
    {
        GridGraph gridGraph = AstarPath.active.data.gridGraph;

        return gridGraph.GetNodesInRegion(bounds);
    }

    public static List<GraphNode> GetNeighboringGraphNodesInBounds(Bounds bounds)
    {
        List<GraphNode> neighbors = new List<GraphNode>();

        foreach(GraphNode graphNode in GetGraphNodesInBounds(bounds))
        {
            foreach(GraphNode neighbor in GetNeighboringGraphNodes(graphNode))
            {
                if(!neighbors.Contains(neighbor))
                {
                    neighbors.Add(neighbor);
                }
            }
        }

        return neighbors;
    }

    public static List<GraphNode> GetNeighboringGraphNodes(GraphNode graphNode)
    {
        GridGraph gridGraph = AstarPath.active.data.gridGraph;

        List<GraphNode> neighbors = new List<GraphNode>();

        graphNode.GetConnections(otherNode =>
        {
            neighbors.Add(otherNode);
        });

        return neighbors;
    }

    public static bool GetClosestGraphNodeToPosition(Vector3 position, out GraphNode result)
    {
        GridGraph gridGraph = AstarPath.active.data.gridGraph;

        NNInfoInternal nnInfo = gridGraph.GetNearest(position, BuildConstraint());

        if (nnInfo.node != null)
        {
            result = nnInfo.node;
            return true;
        }
        else
        {
            result = null;
            return false;
        }
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
