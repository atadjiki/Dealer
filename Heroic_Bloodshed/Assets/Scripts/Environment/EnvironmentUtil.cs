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
        Vector3 offset = new Vector3(0, ENV_TILE_SIZE*2, 0);

        return PerformRaycast(origin + offset, Vector3.down, offset.magnitude);
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
        ABPath path = CalculatePath(origin, destination);

        return path.vectorPath;
    }

    //divide a path into sub-paths, occuring when a character must perform actions
    //such as jumping over an obstacle or climbing a ladder
    public static Queue<List<TileNode>> SubdividePath(ABPath path)
    {
        Queue<List<TileNode>> queue = new Queue<List<TileNode>>();

        List<TileNode> nodes = ConvertGraphToTileNodes(path.path);

        int processed = 0;

        while(processed < nodes.Count)
        {
            List<TileNode> subPath = new List<TileNode>();

            //march down the list and add nodes to our path unless we hit a path interrupt flag
            for(int i = processed; i < nodes.Count; i++)
            {
                TileNode node = nodes[processed];

                subPath.Add(node);
                processed++;

                if (node.pathInterrupt)
                {
                    break;
                }
            }

           queue.Enqueue(subPath);
            
        }

        Debug.Log("Subdivided path into " + queue.Count + " sections");

        return queue;
    }

    public static ABPath CalculatePath(Vector3 origin, Vector3 destination)
    {
        ABPath path = ABPath.Construct(origin, destination);
        AstarPath.StartPath(path,true, true);
        path.BlockUntilCalculated();

        SubdividePath(path);

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

    public static TileConnectionInfo CheckNeighborConnection(Vector3 origin, EnvironmentDirection dir)
    {
        Vector3 direction = GetDirectionVector(dir);
        Vector3 neighborOrigin = GetNeighboringTileLocation(origin, dir);

        TileConnectionInfo info = TileConnectionInfo.Build();
        info.Layer = CheckTileLayer(neighborOrigin);

        Vector3 offset = new Vector3(0, ENV_TILE_SIZE / 2, 0);

        //now check that nothing is in the way between this tile and its neighbor (like walls or corners)
        info.Obstruction = PerformRaycast(origin + offset, direction, direction.magnitude);

        return info;
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
}
