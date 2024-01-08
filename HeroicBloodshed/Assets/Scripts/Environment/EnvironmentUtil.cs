using System;
using System.Collections;
using System.Collections.Generic;
using EPOOutline;
using Pathfinding;
using UnityEngine;
using UnityEngine.EventSystems;
using static Constants;

[Serializable]
public struct EnvironmentCoverData
{
    public Vector3 Direction;
    public EnvironmentObstacleType ObstacleType;

    public static EnvironmentCoverData Build(Vector3 direction, EnvironmentObstacleType obstacleType)
    {
        return new EnvironmentCoverData()
        {
            Direction = direction,
            ObstacleType = obstacleType
        };
    }
}

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
            if (node.Tag == TAG_LAYER_WALL_DEFAULT || node.Tag ==  TAG_LAYER_WALL_SPAWN)
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
            int random = UnityEngine.Random.Range(0, candidates.Count);

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

    public static bool IsWithinCharacterRange(Vector3 destination, CharacterComponent character, out MovementRangeType rangeType)
    {
        Vector3 origin = character.GetWorldLocation();

        ABPath path = ABPath.Construct(origin, destination);

        path.heuristic = Heuristic.Euclidean;
        path.nnConstraint = BuildConstraint();

        AstarPath.StartPath(path, true);

        path.BlockUntilCalculated();

        return IsWithinCharacterRange(0, character, out rangeType);
    }


    public static bool IsWithinCharacterRange(int pathCost, CharacterComponent character, out MovementRangeType rangeType)
    {
        float threshold;

        int AP = character.GetActionPoints();
        float movementRange = character.GetMovementRange();

        if (AP >= GetAbilityCost(AbilityID.MoveFull))
        {
            threshold = (movementRange * 2);
        }
        else
        {
            threshold = movementRange;
        }

        if (pathCost <= threshold)
        {
            if (pathCost <= character.GetMovementRange())
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

    public static List<EnvironmentCoverData> GetNodeCoverData(GraphNode node)
    {
        Vector3 origin = (Vector3)node.position;

        List<EnvironmentCoverData> CoverDirections = new List<EnvironmentCoverData>();

        foreach (KeyValuePair<Vector3,Vector3> pair in GetCardinalNeighbors(node))
        {
            RaycastHit hitInfo;

            if(Physics.Linecast(origin, pair.Value, out hitInfo, LayerMask.GetMask(LAYER_ENV_OBSTACLE)))
            {
                EnvironmentObstacle obstacle = hitInfo.collider.gameObject.GetComponent<EnvironmentObstacle>();

                CoverDirections.Add(EnvironmentCoverData.Build(pair.Key, obstacle.GetObstacleType()));
            }
            if (Physics.Linecast(origin, pair.Value, LayerMask.GetMask(LAYER_ENV_WALL)))
            {
                CoverDirections.Add(EnvironmentCoverData.Build(pair.Key, EnvironmentObstacleType.FullCover));
            }
        }

        return CoverDirections;
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
        if (graphNode.Tag == TAG_LAYER_OBSTACLE || graphNode.Tag ==  TAG_LAYER_CHARACTER)
        {
            return true;
        }

        //do additional check if characters are overlapping anything
        if(EncounterManager.IsActive())
        {
            if(EncounterManager.Instance.GetCurrentState() == EncounterState.CHOOSE_ACTION)
            {
                foreach (CharacterComponent character in EncounterManager.Instance.GetAllCharacters())
                {
                    if(character.GetWorldLocation() == (Vector3) graphNode.position)
                    {
                        return true;
                    }
                }
            }
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


        Debug.Log("Found " + nodes.Count + " with tag " + tag);

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

    public static Dictionary<Vector3, Vector3> GetCardinalNeighbors(GraphNode node)
    {
        Vector3 origin = (Vector3)node.position;

        return new Dictionary<Vector3, Vector3>()
        {
            { Vector3.forward, origin + (Vector3.forward * TILE_SIZE) },
            { Vector3.forward * -1, origin + (Vector3.forward * TILE_SIZE * -1) },
            { Vector3.right, origin + (Vector3.right * TILE_SIZE) },
            { Vector3.right * -1, origin + (Vector3.right * TILE_SIZE * -1) },
        };
    }

    public static int GetDistanceInNodes(Vector3 origin, Vector3 destination)
    {
        float distance = Vector3.Distance(origin, destination) / TILE_SIZE;

        return Mathf.RoundToInt(distance);
    }

    public static bool IsNodeExposed(Vector3 origin)
    {
        GridGraph gridGraph = AstarData.active.data.gridGraph;

        NNInfoInternal nnInfo = gridGraph.GetNearest(origin);

        bool exposed = true;

        if(nnInfo.node != null)
        {
            CharacterComponent currentCharacter = EncounterManager.Instance.GetCurrentCharacter();

            List<CharacterComponent> enemies = EncounterManager.Instance.GetAllCharactersInTeam(GetOpposingTeam(currentCharacter.GetTeam()));

            if(enemies != null && enemies.Count > 0)
            {
                foreach (CharacterComponent enemy in enemies)
                {
                    if (!IsInWeaponRange(enemy, origin))
                    {
                        exposed = false;
                    }
                    else if (enemy.IsAlive())
                    {
                        RaycastHit hitInfo;
                        if (Physics.Linecast(origin, enemy.GetWorldLocation(), out hitInfo, LayerMask.GetMask(LAYER_ENV_OBSTACLE)))
                        {
                            if (hitInfo.collider != null)
                            {
                                exposed = false;
                            }
                        }
                        if (Physics.Linecast(origin, enemy.GetWorldLocation(), out hitInfo, LayerMask.GetMask(LAYER_ENV_WALL)))
                        {
                            if (hitInfo.collider != null)
                            {
                                exposed = false;
                            }
                        }
                    }
                }
            }
        }

        return exposed;
    }

    public static GraphNode GetClosestNode(List<GraphNode> nodes, Vector3 origin)
    {
        if(nodes.Count > 0)
        {
            GraphNode closest = nodes[0];

            foreach(GraphNode node in nodes)
            {
                float currentDistance = Vector3.Distance(origin, (Vector3)closest.position);
                float nodeDistance = Vector3.Distance(origin, (Vector3)node.position);

                if(nodeDistance < currentDistance)
                {
                    closest = node;
                }
            }

            return closest;
        }

        return null;
    }


    public static Vector3 FindClosestNodeInRange(CharacterComponent caster, CharacterComponent target)
    {
        float range = caster.GetWeaponRange() * 2;

        Bounds bounds = new Bounds(target.GetWorldLocation(), new Vector3(range, 0, range));

        List<GraphNode> nodes = GetGraphNodesInBounds(bounds);

        List<GraphNode> validNodes = new List<GraphNode>();

        foreach(GraphNode node in nodes)
        {
            if(!IsGraphNodeOccupied(node))
            {
                validNodes.Add(node);
            }
        }

        GraphNode closestNode = GetClosestNode(validNodes, caster.GetWorldLocation());

        Debug.DrawLine(caster.GetWorldLocation(), (Vector3)closestNode.position, Color.red, 3f);

        return (Vector3)closestNode.position;
    }

    public static bool IsInWeaponRange(CharacterComponent caster, CharacterComponent target)
    {
        int distance = GetDistanceInNodes(caster.GetWorldLocation(), target.GetWorldLocation());
        return distance < caster.GetWeaponRange();
    }

    public static bool IsInWeaponRange(CharacterComponent caster, Vector3 target)
    {
        int distance = GetDistanceInNodes(caster.GetWorldLocation(), target);
        return distance < caster.GetWeaponRange();
    }
}
