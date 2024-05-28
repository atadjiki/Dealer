using System;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;
using static Constants;


[Serializable]
public struct EnvironmentTileRaycastInfo
{
    public Vector3 position;
    public EnvironmentLayer layer;

    public static EnvironmentTileRaycastInfo Build()
    {
        return new EnvironmentTileRaycastInfo()
        {
            position = Vector3.zero,
            layer = EnvironmentLayer.NONE
        };
    }
}

[Serializable]
public struct EnvironmentTileConnectionInfo
{
    public EnvironmentLayer Layer;
    public EnvironmentLayer Obstruction;

    public static EnvironmentTileConnectionInfo Build()
    {
        return new EnvironmentTileConnectionInfo()
        {
            Layer = EnvironmentLayer.NONE,
            Obstruction = EnvironmentLayer.NONE,
        };
    }

    public bool IsValid()
    {
        return IsLayerTraversible(Layer) && Obstruction == EnvironmentLayer.NONE;
    }

    public bool ProvidesCover()
    {
        return IsLayerObstacle(Layer) && IsLayerCover(Obstruction);
    }
}

public class EnvironmentTileConnectionMap : Dictionary<EnvironmentDirection, EnvironmentTileConnectionInfo>
{
    public EnvironmentTileConnectionMap()
    {
        foreach (EnvironmentDirection dir in GetAllDirections())
        {
            Add(dir, EnvironmentTileConnectionInfo.Build());
        }
    }
}

public class EnvironmentUtil
{
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

    public static List<Vector3> GetTileNeighbors(Vector3 origin)
    {
        List<Vector3> Neighbors = new List<Vector3>();

        EnvironmentLayer tileLayer = CheckTileLayer(origin);

        if(IsLayerTraversible(tileLayer))
        {
            foreach (EnvironmentDirection dir in GetAllDirections())
            {
                Neighbors.Add(GetNeighboringTileLocation(origin, dir));
            }
        }

        return Neighbors;
    }

    public static EnvironmentTileConnectionMap GenerateNeighborMap(Vector3 origin)
    {
        EnvironmentTileConnectionMap neighborMap = new EnvironmentTileConnectionMap();

        foreach (EnvironmentDirection dir in GetAllDirections())
        {
            neighborMap[dir] = CheckNeighborConnection(origin, dir);
        }

        return neighborMap;
    }

    public static Bounds GetEnvironmentBounds()
    {

        TileGraph graph = GetEnvironmentGraph();

        Vector3 center = Vector3.zero;

        Vector3 size = new Vector3(ENV_TILE_SIZE * graph.Width, 0, ENV_TILE_SIZE * graph.Width);

        return new Bounds(center, size);
    }

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

    public static bool GetNearestTile(Vector3 origin, out Vector3 nearest)
    {
        NNInfo info = AstarPath.active.GetNearest(origin);

        if(info.node != null)
        {
            nearest = ((Vector3)info.node.position);
            return true;
        }

        nearest = Vector3.zero;
        return false;
    }

    public static Vector3 GetRandomTile()
    {
        TileGraph graph = GetEnvironmentGraph();

        PointNode node = graph.GetRandomNode();

        if (node != null)
        {
            return (Vector3)node.position;
        }
        else
        {
            Debug.LogError("Random node was null!");
            return Vector3.zero;
        }
    }

    public static bool IsTileCoverAdjaecent(Vector3 origin)
    {
        EnvironmentTileConnectionMap neighborMap = EnvironmentUtil.GenerateNeighborMap(origin);

        foreach (EnvironmentDirection dir in GetCardinalDirections())
        {
            EnvironmentTileConnectionInfo info = neighborMap[dir];

            if (IsLayerCover(info.Obstruction))
            {
                return true;
            }
        }

        return false;
    }

    public static List<Vector3> GetCoverAdjaecentTiles()
    {
        TileGraph graph = GetEnvironmentGraph();

        List<Vector3> tiles = new List<Vector3>();

        foreach(PointNode node in graph.GetWalkableNodes())
        {
            Vector3 nodePos = ((Vector3)node.position);

            if(IsTileCoverAdjaecent(nodePos))
            {
                tiles.Add(nodePos);
            }
        }

        return tiles;
    }

    public static Vector3 GetClosest(Vector3 origin, List<Vector3> points)
    {
        Vector3 closest = Vector3.positiveInfinity;

        foreach(Vector3 point in points)
        {
            if(Vector3.Distance(origin, point) <= Vector3.Distance(origin, closest))
            {
                closest = point;
            }
        }

        return closest;
    }

    public static Vector3 GetClosestTileWithCover(Vector3 origin)
    {
        return GetClosest(origin, GetCoverAdjaecentTiles());
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

        return GetCharacterRadius(rangeType, character);
    }

    public static List<Vector3> GetCharacterRadius(MovementRangeType rangeType, CharacterComponent character)
    {
        return GetTilesWithinRange(character.GetWorldLocation(), character.GetRange(rangeType));
    }

    public static List<Vector3> GetTilesWithinRange(Vector3 origin, int range)
    {
        List<Vector3> tiles = new List<Vector3>();

        int gScore = CalculateGScore(range);

        ConstantPath cpath = ConstantPath.Construct(origin, gScore);
        AstarPath.StartPath(cpath);
        cpath.BlockUntilCalculated();

        foreach(GraphNode node in cpath.allNodes)
        {
            tiles.Add(((Vector3)node.position));
        }

        return tiles;
    }

    public static bool IsWithinCharacterRange(CharacterComponent character, Vector3 location, MovementRangeType rangeType)
    {
        CharacterDefinition def = ResourceUtil.GetCharacterData(character.GetID());

        List<Vector3> range = GetTilesWithinRange(character.GetWorldLocation(), character.GetRange(rangeType));

        return range.Contains(location);
    }

    public static List<Vector3> CalculatePath(Vector3 origin, Vector3 destination)
    {
        ABPath path = ABPath.Construct(origin, destination);
        AstarPath.StartPath(path);
        path.BlockUntilCalculated();

        return path.vectorPath;
    }

    public static Dictionary<MovementRangeType, List<Vector3>> GetCharacterRangeMap(CharacterComponent character)
    {
        Dictionary <MovementRangeType, List <Vector3>> map = new Dictionary<MovementRangeType, List<Vector3>>()
        {
            { MovementRangeType.HALF, GetCharacterRadius(MovementRangeType.HALF, character) },
            { MovementRangeType.FULL, GetCharacterRadius(MovementRangeType.FULL, character) },
        };

        foreach(MovementRangeType rangeType in map.Keys)
        {
            Debug.Log(map[rangeType].Count + " tiles in radius for " + rangeType.ToString());
        }

        return map;
    }

    public static bool GetTileBeneathMouse(out EnvironmentTileRaycastInfo info)
    {
        info = EnvironmentTileRaycastInfo.Build();

        TileGraph graph = GetEnvironmentGraph();

        if (graph != null && Camera.main != null)
        {
            Vector3 mousePosition = Input.mousePosition;

            Ray ray = Camera.main.ScreenPointToRay(mousePosition);

            foreach (RaycastHit hit in Physics.RaycastAll(ray, 100))
            {
                if (hit.collider.gameObject.layer == LAYER_GROUND)
                {
                    Vector3 nearest;
                    if (GetNearestTile(hit.point, out nearest))
                    {
                        info.layer = CheckTileLayer(nearest);
                        info.position = nearest;
                        return true;
                    }
                }
            }
        }

        return false;
    }

    public static GameObject CreateTileQuad(Material material, Transform parent)
    {
        return CreateTileQuad(material, parent, GetTileScale());
    }

    public static GameObject CreateTileQuad(Material material, Transform parent, Vector3 scale)
    {
        GameObject quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
        quad.transform.parent = parent;

        quad.transform.localEulerAngles = new Vector3(90, 0, 0);
        quad.transform.localScale = scale;

        GameObject.Destroy(quad.GetComponent<MeshCollider>());

        quad.layer = LAYER_DECAL;

        MeshRenderer renderer = quad.GetComponent<MeshRenderer>();

        renderer.material = material;

        return quad;
    }

    private static EnvironmentTileConnectionInfo CheckNeighborConnection(Vector3 origin, EnvironmentDirection dir)
    {
        Vector3 direction = GetDirectionVector(dir);
        Vector3 neighborOrigin = GetNeighboringTileLocation(origin, dir);

        EnvironmentTileConnectionInfo info = EnvironmentTileConnectionInfo.Build();
        info.Layer = CheckTileLayer(neighborOrigin);

        Vector3 offset = new Vector3(0, ENV_TILE_SIZE / 2, 0);

        //now check that nothing is in the way between this tile and its neighbor (like walls or corners)
        info.Obstruction = PerformRaycast(origin + offset, direction, direction.magnitude);

        return info;
    }
}
