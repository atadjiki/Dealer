using System;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

public class EnvironmentUtil
{
    public static EnvironmentLayer CheckTileLayer(Vector3 origin)
    {
        Vector3 offset = new Vector3(0, 3, 0);

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
                bool connection = PerformNeighborCheck(origin, dir);

                Neighbors.Add(GetNeighboringTileLocation(origin, dir));
            }
        }

        return Neighbors;
    }

    public static EnvironmentTileNeighborMap GenerateNeighborMap(Vector3 origin)
    {
        EnvironmentLayer tileLayer = CheckTileLayer(origin);

        EnvironmentTileNeighborMap neighborMap = new EnvironmentTileNeighborMap();

        if (IsLayerTraversible(tileLayer))
        {
            foreach (EnvironmentDirection dir in GetAllDirections())
            {
                neighborMap[dir] = PerformNeighborCheck(origin, dir);
            }
        }

        return neighborMap;
    }

    private static bool PerformNeighborCheck(Vector3 origin, EnvironmentDirection dir)
    {
        Vector3 direction = GetDirectionVector(dir);
        Vector3 neighborOrigin = GetNeighboringTileLocation(origin, dir);
        EnvironmentLayer neighborLayer = CheckTileLayer(neighborOrigin);

        //first check that the neighbor is a valid tile in the first place

        Vector3 offset = new Vector3(0, ENV_TILE_SIZE / 2, 0);

        if (IsLayerTraversible(neighborLayer))
        {
            //now check that nothing is in the way between this tile and it's neighbor (like walls or corners)
            if(Physics.Raycast(origin + offset, direction, direction.magnitude))
            {
                return false;
            }

            return true;
        }
        return false;
    }

    public static Vector3 GetNeighboringTileLocation(Vector3 origin, EnvironmentDirection dir)
    {
        Vector3 direction = GetDirectionVector(dir);

        return origin + direction;
    }

    public static Vector3 CalculateTileOrigin(int Row, int Column)
    {
        Vector3 tilePivot = new Vector3(Row * ENV_TILE_SIZE, 0, Column * ENV_TILE_SIZE);

        return tilePivot + new Vector3(ENV_TILE_SIZE / 2, 0, ENV_TILE_SIZE / 2);
    }

    public static Vector2 CalculateTileCoordinates(Vector3 origin)
    {
        origin -= new Vector3(ENV_TILE_SIZE / 2, 0, ENV_TILE_SIZE / 2);

        origin /= ENV_TILE_SIZE;

        return new Vector2(origin.x, origin.z);
    }

    public static EnvironmentLayer GetLayer(int Layer)
    {
        if (Layer == LAYER_GROUND)
        {
            return EnvironmentLayer.Ground;
        }
        else if(Layer == LAYER_OBSTACLE_HALF)
        {
            return EnvironmentLayer.Obstacle_Half;
        }
        else if(Layer == LAYER_OBSTACLE_FULL)
        {
            return EnvironmentLayer.Obstacle_Full;
        }

        return EnvironmentLayer.None;
    }

    public static bool IsObstacleLayer(EnvironmentLayer Layer)
    {
        return (Layer == EnvironmentLayer.Obstacle_Full || Layer == EnvironmentLayer.Obstacle_Half);
    }

    public static Vector3 GetDirectionVector(EnvironmentDirection Direction)
    {
        Vector3 vector;

        switch (Direction)
        {
            case EnvironmentDirection.NORTH:
                vector = Vector3.forward;
                break;
            case EnvironmentDirection.SOUTH:
                vector = Vector3.back;
                break;
            case EnvironmentDirection.EAST:
                vector = Vector3.left;
                break;
            case EnvironmentDirection.WEST:
                vector = Vector3.right;
                break;

            case EnvironmentDirection.NORTH_EAST:
                vector = Vector3.forward + Vector3.left;
                break;
            case EnvironmentDirection.NORTH_WEST:
                vector = Vector3.forward + Vector3.right;
                break;
            case EnvironmentDirection.SOUTH_EAST:
                vector = Vector3.back + Vector3.left;
                break;
            case EnvironmentDirection.SOUTH_WEST:
                vector = Vector3.back + Vector3.right;
                break;

            default:
                return Vector3.zero;
        }

        return vector *= GetDirectionMagnitude(Direction);
    }

    public static float GetDirectionMagnitude(EnvironmentDirection Direction)
    {
        if (IsCardinalDirection(Direction))
        {
            return ENV_TILE_SIZE;
        }
        else
        {
            return Mathf.Sqrt(Mathf.Pow(ENV_TILE_SIZE, 2));
        }
    }

    public static bool IsCardinalDirection(EnvironmentDirection Direction)
    {
        switch(Direction)
        {
            case EnvironmentDirection.NORTH_EAST:
            case EnvironmentDirection.NORTH_WEST:
            case EnvironmentDirection.SOUTH_EAST:
            case EnvironmentDirection.SOUTH_WEST:
                return false;
            default:
                return true;
        }
    }

    public static bool IsLayerTraversible(EnvironmentLayer Layer)
    {
        switch(Layer)
        {
            case EnvironmentLayer.Obstacle_Full:
            case EnvironmentLayer.Obstacle_Half:
            case EnvironmentLayer.None:
                return false;
            default:
                return true;
        }
    }

    public static Color GetLayerDebugColor(EnvironmentLayer Layer, bool ShowInvalidLayer = true)
    {
        switch (Layer)
        {
            case EnvironmentLayer.Ground:
                return Color.green;
            case EnvironmentLayer.Obstacle_Full:
            case EnvironmentLayer.Obstacle_Half:
                return Color.yellow;
            case EnvironmentLayer.None:
                {
                    if(ShowInvalidLayer)
                    {
                        return Color.red;
                    }
                    else
                    {
                        return Color.clear;
                    }
                }

            default:
                return Color.clear;
        }
    }

    public static Color GetConnectionDebugColor(bool Flag, bool ShowInvalidConnection = true)
    {
        if (Flag == true)
        {
            return Color.green;
        }
        else if(ShowInvalidConnection)
        {
            return Color.red;
        }
        else
        {
            return Color.clear;
        }
    }

    public static Array GetAllDirections()
    {
        return Enum.GetValues(typeof(EnvironmentDirection));
    }
}
