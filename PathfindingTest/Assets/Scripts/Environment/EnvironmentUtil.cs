using System;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

[Serializable]
public struct EnvironmentTileConnectionInfo
{
    public EnvironmentLayer Layer;
    public bool IsObstructed;

    public static EnvironmentTileConnectionInfo Build()
    {
        return new EnvironmentTileConnectionInfo()
        {
            Layer = EnvironmentLayer.None,
            IsObstructed = false,
        };
    }

    public bool IsValid()
    {
        return IsLayerTraversible(Layer) && !IsObstructed;
    }

    public bool ProvidesCover()
    {
        return IsLayerObstacle(Layer) && IsObstructed;
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
                int layer = hitInfo.collider.gameObject.layer;

                EnvironmentLayer state = GetLayer(layer);

                return state;
            }
        }

        return EnvironmentLayer.None;
    }

    public static List<Vector3> GetTileNeighbors(Vector3 origin)
    {
        List<Vector3> Neighbors = new List<Vector3>();

        EnvironmentLayer tileLayer = CheckTileLayer(origin);

        if(IsLayerTraversible(tileLayer))
        {
            foreach (EnvironmentDirection dir in GetAllDirections())
            {
                EnvironmentTileConnectionInfo info = CheckNeighborConnection(origin, dir);

                if(info.IsValid())
                {
                    Neighbors.Add(GetNeighboringTileLocation(origin, dir));
                }
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

    private static EnvironmentTileConnectionInfo CheckNeighborConnection(Vector3 origin, EnvironmentDirection dir)
    {
        Vector3 direction = GetDirectionVector(dir);
        Vector3 neighborOrigin = GetNeighboringTileLocation(origin, dir);

        EnvironmentTileConnectionInfo info = EnvironmentTileConnectionInfo.Build();
        info.Layer = CheckTileLayer(neighborOrigin);

        Vector3 offset = new Vector3(0, ENV_TILE_SIZE / 2, 0);

        //now check that nothing is in the way between this tile and it's neighbor (like walls or corners)
        info.IsObstructed = Physics.Raycast(origin + offset, direction, direction.magnitude);

        return info;
    }
}
