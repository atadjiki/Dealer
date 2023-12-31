using System.Collections;
using System.Collections.Generic;
using EPOOutline;
using Pathfinding;
using UnityEngine;
using UnityEngine.EventSystems;
using static Constants;

public class EnvironmentUtil : MonoBehaviour
{
    public static void ProcessEnvironment()
    {
        Debug.Log("Scanning NavMesh");
        GridGraph gridGraph = AstarPath.active.data.gridGraph;
        AstarPath.active.Scan(gridGraph);

        //remove connections between nodes intersected by walls
        foreach (GraphNode node in gridGraph.nodes)
        {
            if (node.Tag == TAG_LAYER_WALL)
            {
                foreach (GraphNode neighbor in GetNeighboringNodes(node))
                {
                    Vector3 origin = (Vector3)node.position;
                    Vector3 destination = (Vector3)neighbor.position;

                    if (Physics.Linecast(origin, destination, LayerMask.GetMask(LAYER_ENV_WALL)))
                    {
                        node.RemoveConnection(neighbor);
                    }
                }
            }
        }
    }

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

    public static bool GetRandomUnoccupiedNode(out GraphNode result)
    {
        GridGraph gridGraph = AstarPath.active.data.gridGraph;

        List<GraphNode> candidates = new List<GraphNode>();

        foreach(GraphNode graphNode in gridGraph.nodes)
        {
            if(IsGraphNodeFree(graphNode) && graphNode.Walkable)
            {
                candidates.Add(graphNode);
            }
        }

        if(candidates.Count > 0)
        {
            int random = Random.Range(0, candidates.Count);

            result = candidates[random];
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

    public static List<GraphNode> GetNeighboringNodes(GraphNode node)
    {
        List<GraphNode> neighbors = new List<GraphNode>();
        node.GetConnections(neighbors.Add);
        return neighbors;
    }

    public static void PaintNeighboringNodesAs(GraphNode graphNode, EnvironmentNodeTagType tagType)
    {
        List<GraphNode> neighbors = new List<GraphNode>();
        graphNode.GetConnections(neighbors.Add);

        foreach (GraphNode neighbor in neighbors)
        {
            if (neighbor.Tag != graphNode.Tag && neighbor.Walkable)
            {
                PaintNodeAs(neighbor, tagType);
            }
        }
    }

    public static void PaintNodeAs(GraphNode graphNode, EnvironmentNodeTagType tagType)
    {
        switch(tagType)
        {
            case EnvironmentNodeTagType.Obstacle:
                graphNode.Tag = TAG_LAYER_OBSTACLE;
                break;
            case EnvironmentNodeTagType.Character:
                graphNode.Tag = TAG_LAYER_CHARACTER;
                break;
            case EnvironmentNodeTagType.SpawnLocation:
                graphNode.Tag = TAG_LAYER_SPAWNLOCATION;
                break;
            case EnvironmentNodeTagType.Wall:
                graphNode.Tag = TAG_LAYER_WALL;
                break;
            case EnvironmentNodeTagType.Cover:
                graphNode.Tag = TAG_LAYER_COVER;
                break;
            default:
                break;
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

    public static bool IsPositionOccupied(Vector3 worldLocation)
    {
        GraphNode graphNode;
        if (GetClosestGraphNodeToPosition(worldLocation, out graphNode))
        {
            return IsGraphNodeOccupied(graphNode);
        }
        return false;
    }

    public static bool IsGraphNodeFree(GraphNode graphNode)
    {
        return !IsGraphNodeOccupied(graphNode);
    }

    public static bool IsGraphNodeOccupied(GraphNode graphNode)
    {
        if (graphNode.Tag == TAG_LAYER_OBSTACLE || graphNode.Tag == TAG_LAYER_CHARACTER)
        {
            return true;
        }

        return false;
    }

    public static bool IsPositionFree(Vector3 worldLocation)
    {
        return !IsPositionOccupied(worldLocation);
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

    public static bool IsPathPossible()
    {
        return false;
    }

    public static List<GraphNode> GetNodesWithTag(uint tag)
    {
        List<GraphNode> nodes = new List<GraphNode>();

        GridGraph gridGraph = AstarData.active.data.gridGraph;

        foreach(GraphNode node in gridGraph.nodes)
        {
            if(node.Tag == tag)
            {
                nodes.Add(node);
            }
        }

        return nodes;
    }

    public static void AddOutline(GameObject gameObject, Color color, float opacity)
    {
        Outlinable outline = gameObject.AddComponent<Outlinable>();

        foreach (Renderer renderer in gameObject.GetComponentsInChildren<Renderer>())
        {
            OutlineTarget target = new OutlineTarget(renderer);
            outline.TryAddTarget(target);
        }

        Color outlineColor = color;
        outlineColor.a = opacity;
        outline.OutlineParameters.Color = outlineColor;
        outline.OutlineParameters.DilateShift = 0.25f;
        outline.OutlineParameters.BlurShift = 0.25f;
    }
}
