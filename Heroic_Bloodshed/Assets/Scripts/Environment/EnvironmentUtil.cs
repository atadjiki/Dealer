using System;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;
using static Constants;

public class EnvironmentUtil
{
    //Global function to access the active tile graph
    public static TileGraph GetEnvironmentGraph()
    {
        TileGraph graph = (TileGraph)AstarPath.active.data.FindGraphOfType(typeof(TileGraph));

        return graph;
    }

    public static void Scan()
    {
        Debug.Log("Environment Scan");

        TileGraph graph = GetEnvironmentGraph();

        AstarPath.active.Scan(graph);
    }

    //Raycasting
    public static EnvironmentLayer CheckTileLayer(Vector3 origin)
    {
        Vector3 offset = new Vector3(0, (ENV_MAX_LVL * ENV_TILE_SIZE * ENV_LVL_STEP) + 1, 0);

        Ray ray = new Ray(origin + offset, Vector3.down);

        EnvironmentLayer layer = EnvironmentLayer.NONE;

        foreach (RaycastHit hitInfo in Physics.RaycastAll(ray, offset.magnitude))
        {
            float height = hitInfo.collider.transform.position.y;

            if(GetElevation(height) == GetElevation(origin.y))
            {
                if (hitInfo.collider != null)
                {
                    int layerMask = hitInfo.collider.gameObject.layer;

                    if(layer == EnvironmentLayer.NONE || layer == EnvironmentLayer.GROUND)
                    {
                        layer = GetLayer(layerMask);
                    }
                }
            }
        }

        return layer;
    }

    public static EnvironmentLayer PerformRaycast(Vector3 origin, Vector3 direction, float range)
    {
        Ray ray = new Ray(origin, direction);
        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo, range))
        {
            if(hitInfo.collider != null)
            {
                int layerMask = hitInfo.collider.gameObject.layer;

                EnvironmentLayer layer = GetLayer(layerMask);

                return layer;
            }
        }

        return EnvironmentLayer.NONE;
    }

    public static bool GetNodeBeneathMouse(out TileNode node)
    {
        TileGraph graph = GetEnvironmentGraph();

        if (graph != null && Camera.main != null)
        {
            Vector3 mousePosition = Input.mousePosition;

            Ray ray = Camera.main.ScreenPointToRay(mousePosition);

            foreach (RaycastHit hit in Physics.RaycastAll(ray, 100))
            {
                if (hit.collider.gameObject.layer == LAYER_GROUND)
                {
                    if (GetNearestNode(hit.point, out node))
                    {
                        return true;
                    }

                }
            }
        }

        node = null;
        return false;
    }

    public static bool GetNearestNode(Vector3 origin, out TileNode node)
    {
        NNInfo info = AstarPath.active.GetNearest(origin);

        if(info.node != null)
        {
            node = (TileNode)info.node;
            return true;
        }

        node = null;
        return false;
    }

    public static TileNode GetRandomNode()
    {
        TileGraph graph = GetEnvironmentGraph();

        TileNode node = graph.GetRandomNode();

        return node;
    }

    public static Vector3 GetRandomLocation()
    {
        return (Vector3) GetRandomNode().position;
    }

    public static List<Vector3> CalculateVectorPath(Vector3 origin, Vector3 destination)
    {
        ABPath abPath = CalculatePath(origin, destination);

        Queue<MovementInfo> pathQueue = CreatePathQueue(abPath);

        List<Vector3> vectors = new List<Vector3>();

        foreach (MovementInfo info in pathQueue)
        {
            if (info.Type == MovementType.MOVE)
            {
                vectors.AddRange(info.GetVectors());
            }
            else if (info.Type == MovementType.VAULT_OBSTACLE || info.Type == MovementType.VAULT_WALL)
            {
                Vector3 start = (Vector3)info.Nodes[0].position;
                Vector3 end = (Vector3)info.Nodes[1].position;
                Vector3 midPoint = ((start + end) / 2) + new Vector3(0, ENV_TILE_SIZE / 2, 0);

                vectors.Add(start);
                vectors.Add(midPoint);
                vectors.Add(end);

            }
        }

        return vectors;
    }

    private static MovementInfo FindNextSubPath(ref List<TileNode> nodes)
    {
        MovementInfo info = new MovementInfo();

        for(int i = 0; i < nodes.Count; i++)
        {
            TileNode current = nodes[i];

            if((i + 1) < nodes.Count)
            {
                TileNode next = nodes[i + 1];

                info.Nodes.Add(current);

                if (current.HasTransitionTo(next))
                {
                    if (i == 0)
                    {
                        MovementType pathType = current.GetTransiton(next);
                        MovementInfo jumpInfo = new MovementInfo(current, next, pathType);

                        nodes.Remove(current);

                        return jumpInfo;
                    }
                    break;
                }
            }
            else
            {
                info.Nodes.Add(current);
            }
        }

        for(int i = 0; i < info.Nodes.Count -1; i++)
        {
            nodes.Remove(info.Nodes[i]);
        }

        if(nodes.Count == 1)
        {
            nodes.Clear();
        }

        return info;
    }

    //divide a path into sub-paths, occuring when a character must perform actions
    //such as jumping over an obstacle or climbing a ladder
    public static Queue<MovementInfo> CreatePathQueue(ABPath path)
    {
        Queue<MovementInfo> queue = new Queue<MovementInfo>();

        List<TileNode> nodes = ConvertGraphToTileNodes(path.path);

        while(nodes.Count > 0)
        {
            MovementInfo subPath = FindNextSubPath(ref nodes);

            if(subPath.Nodes.Count > 1)
            {
                queue.Enqueue(subPath);
            }
        }

        return queue;
    }

    private static ABPath CalculatePath(Vector3 origin, Vector3 destination)
    {
        ABPath path = ABPath.Construct(origin, destination);
        AstarPath.StartPath(path,true, true);
        path.BlockUntilCalculated();

        CreatePathQueue(path);

        return path;
    }

    public static List<Vector3> GetVectorsWithinRange(Vector3 origin, int range)
    {
        ConstantPath cpath = CreateConstantPath(origin, range);

        List<Vector3> nodes = new List<Vector3>();

        foreach (GraphNode node in cpath.allNodes)
        {
            nodes.Add((Vector3)node.position);
        }

        return nodes;
    }

    public static List<TileNode> GetNodesWithinRange(Vector3 origin, int range)
    {
        ConstantPath cpath = CreateConstantPath(origin, range);

        List<TileNode> nodes = new List<TileNode>();

        foreach(GraphNode node in cpath.allNodes)
        {
            nodes.Add((TileNode)node);
        }

        return nodes;
    }

    private static ConstantPath CreateConstantPath(Vector3 origin, int range)
    {
        int gScore = CalculateGScore(range);

        NNConstraint constraint = new NNConstraint();
        constraint.constrainWalkability = true;

        ConstantPath cpath = ConstantPath.Construct(origin, gScore);
        cpath.heuristicScale = 1;
        cpath.heuristic = Heuristic.DiagonalManhattan;
        cpath.nnConstraint = constraint;

        AstarPath.StartPath(cpath);
        cpath.BlockUntilCalculated();

        return cpath;
    }

    public static List<Vector3> GetCharacterMaxRadius(CharacterComponent character)
    {
        MovementRangeType rangeType;
        if(character.CanAffordAbility(AbilityID.MOVE_FULL))
        {
            rangeType = MovementRangeType.FULL;
        }
        else if(character.CanAffordAbility(AbilityID.MOVE_HALF))
        {
            rangeType = MovementRangeType.HALF;
        }
        else
        {
            rangeType = MovementRangeType.NONE;
        }

        return GetCharacterVectorRadius(rangeType, character);
    }

    public static List<Vector3> GetCharacterVectorRadius(MovementRangeType rangeType, CharacterComponent character)
    {
        return GetVectorsWithinRange(character.GetWorldLocation(), character.GetRange(rangeType));
    }

    public static bool IsWithinCharacterMaxRange(CharacterComponent character, Vector3 location)
    {
        List<Vector3> range = GetCharacterMaxRadius(character);

        return range.Contains(location);
    }

    public static Dictionary<MovementRangeType, List<Vector3>> GetCharacterRangeMap(CharacterComponent character)
    {
        Dictionary <MovementRangeType, List <Vector3>> map = new Dictionary<MovementRangeType, List<Vector3>>()
        {
            { MovementRangeType.HALF, GetCharacterVectorRadius(MovementRangeType.HALF, character) },
            { MovementRangeType.FULL, GetCharacterVectorRadius(MovementRangeType.FULL, character) },

        };

        foreach(MovementRangeType rangeType in map.Keys)
        {
            Debug.Log(map[rangeType].Count + " tiles in radius for " + rangeType.ToString());
        }

        return map;
    }

    public static List<TileNode> ConvertGraphToTileNodes(List<GraphNode> graphNodes)
    {
        List<TileNode> tileNodes = new List<TileNode>();

        foreach(GraphNode graphNode in graphNodes)
        {
            TileNode tileNode = (TileNode)graphNode;

            tileNodes.Add(tileNode);
        }

        return tileNodes;
    }

    public static bool GetCostBetweenNodes(TileNode start, TileNode end, out uint cost)
    {
        foreach(Connection connection in start.connections)
        {
            if(connection.node == end)
            {
                cost = connection.cost;
                return true;
            }
        }

        cost = 0;
        return false;
    }
}
