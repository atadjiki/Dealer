using System;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public static partial class Constants
{
    public static float ENV_TILE_SIZE = 1.5f;

    public static int LAYER_GROUND = LayerMask.NameToLayer("GROUND");
    public static int LAYER_OBSTACLE_HALF = LayerMask.NameToLayer("OBSTACLE_HALF");
    public static int LAYER_OBSTACLE_FULL = LayerMask.NameToLayer("OBSTACLE_FULL");
    public static int LAYER_WALL = LayerMask.NameToLayer("WALL");
    public static int LAYER_CHARACTER = LayerMask.NameToLayer("CHARACTER");

    public static Vector3 GetNeighboringTileLocation(Vector3 origin, EnvironmentDirection dir)
    {
        Vector3 direction = GetDirectionVector(dir);

        return origin + direction;
    }

    public static Int2 GetNeighboringTileCoordinates(Vector3 origin, EnvironmentDirection dir)
    {
        Vector3 neighborOrigin = GetNeighboringTileLocation(origin, dir);

        return CalculateTileCoordinates(neighborOrigin);
    }

    public static Vector3 CalculateTileOrigin(int Row, int Column)
    {
        Vector3 tilePivot = new Vector3(Row * ENV_TILE_SIZE, 0, Column * ENV_TILE_SIZE);

        return tilePivot + new Vector3(ENV_TILE_SIZE / 2, 0, ENV_TILE_SIZE / 2);
    }

    public static Vector3[] CalculateTileEdge(Vector3 origin, EnvironmentDirection dir)
    {
        float length = ENV_TILE_SIZE / 2;

        if(dir == EnvironmentDirection.NORTH)
        {
            return new Vector3[]
            {
                origin + new Vector3(  length, 0, length ),
                origin + new Vector3( -length, 0, length )
            };
        }
        else if(dir == EnvironmentDirection.SOUTH)
        {
            return new Vector3[]
            {
                origin + new Vector3(  length, 0, -length ),
                origin + new Vector3( -length, 0, -length )
            };
        }
        else if (dir == EnvironmentDirection.WEST)
        {
            return new Vector3[]
            {
                origin + new Vector3(  length, 0,  length ),
                origin + new Vector3(  length, 0, -length )
            };
        }
        else if (dir == EnvironmentDirection.EAST)
        {
            return new Vector3[]
            {
                origin + new Vector3(  -length, 0,  length ),
                origin + new Vector3(  -length, 0, -length )
            };
        }

        return null;
    }

    public static Vector3 GetCoverNormal(EnvironmentDirection Direction)
    {
        return -1 * GetDirectionVector(Direction);
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

    public static Int2 CalculateTileCoordinates(Vector3 origin)
    {
        origin -= new Vector3(ENV_TILE_SIZE / 2, 0, ENV_TILE_SIZE / 2);

        origin /= ENV_TILE_SIZE;

        return new Int2((int)origin.x, (int)origin.z);
    }

    public static bool IsLayerObstacle(EnvironmentLayer Layer)
    {
        return (Layer == EnvironmentLayer.OBSTACLE_FULL || Layer == EnvironmentLayer.OBSTACLE_HALF);
    }

    public static bool IsLayerCover(EnvironmentLayer Layer)
    {
        return (Layer == EnvironmentLayer.OBSTACLE_FULL || Layer == EnvironmentLayer.OBSTACLE_HALF || Layer == EnvironmentLayer.WALL);
    }

    public static bool IsCardinalDirection(EnvironmentDirection Direction)
    {
        switch (Direction)
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
        switch (Layer)
        {
            case EnvironmentLayer.OBSTACLE_HALF:
            case EnvironmentLayer.OBSTACLE_FULL:
            case EnvironmentLayer.NONE:
                return false;
            default:
                return true;
        }
    }

    public static bool AreValidCoordinates(Int2 coords, int width)
    {
        if (coords.x < 0 || coords.y < 0)
        {
            return false;
        }

        if (coords.x >= width || coords.y >= width)
        {
            return false;
        }

        return true;
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

    public static Color GetLayerDebugColor(EnvironmentLayer Layer, bool ShowTraversibles = true, bool ShowNonTraversibles = true)
    {
        switch (Layer)
        {
            case EnvironmentLayer.GROUND:
                if (ShowTraversibles)
                {
                    return Color.green;
                }
                else
                {
                    return Color.clear;
                }
            case EnvironmentLayer.OBSTACLE_FULL:
                return Color.red;
            case EnvironmentLayer.OBSTACLE_HALF:
                return Color.yellow;
            case EnvironmentLayer.CHARACTER:
                return Color.cyan;
            case EnvironmentLayer.NONE:
                {
                    if (ShowNonTraversibles)
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

    public static Color GetConnectionDebugColor(EnvironmentTileConnectionInfo info, bool ShowValidConnections = true, bool ShowInvalidConnections = true)
    {
        if (ShowValidConnections && info.IsValid())
        {
            return Color.green;
        }

        if (ShowInvalidConnections && !info.IsValid())
        {
            return Color.red;
        }

        return Color.clear;
    }

    public static Color GetCoverDebugColor(EnvironmentCover cover)
    {
        switch(cover)
        {
            case EnvironmentCover.HALF:
                return Color.yellow;
            case EnvironmentCover.FULL:
                return Color.magenta;
            default:
                return Color.clear;
        }
    }

    public static float GetCoverHeight(EnvironmentCover cover)
    {
        switch(cover)
        {
            case EnvironmentCover.HALF:
                return ENV_TILE_SIZE / 2;
            case EnvironmentCover.FULL:
                return ENV_TILE_SIZE;
            default:
                return 0;
        }
    }

    public static EnvironmentCover GetCoverType(EnvironmentTileConnectionInfo info)
    {
        if (info.Layer == EnvironmentLayer.OBSTACLE_HALF)
        {
            return EnvironmentCover.HALF;
        }
        else if (info.Layer == EnvironmentLayer.OBSTACLE_FULL || IsLayerCover(info.Obstruction))
        {
            return EnvironmentCover.FULL;
        }
        else
        {
            return EnvironmentCover.NONE;
        }
    }

    public static Array GetAllDirections()
    {
        return Enum.GetValues(typeof(EnvironmentDirection));
    }

    public static int GetDirectionCount()
    {
        return GetAllDirections().Length;
    }

    public static List<EnvironmentDirection> GetCardinalDirections()
    {
        return new List<EnvironmentDirection>()
        {
            EnvironmentDirection.NORTH,
            EnvironmentDirection.SOUTH,
            EnvironmentDirection.EAST,
            EnvironmentDirection.WEST
        };
    }

    public static EnvironmentLayer GetLayer(int Layer)
    { 

        if (Layer == LAYER_GROUND)
        {
            return EnvironmentLayer.GROUND;
        }
        else if (Layer == LAYER_OBSTACLE_HALF)
        {
            return EnvironmentLayer.OBSTACLE_HALF;
        }
        else if (Layer == LAYER_OBSTACLE_FULL)
        {
            return EnvironmentLayer.OBSTACLE_FULL;
        }
        else if (Layer == LAYER_WALL)
        {
            return EnvironmentLayer.WALL;
        }
        else if (Layer == LAYER_CHARACTER)
        {
            return EnvironmentLayer.CHARACTER;
        }
        else
        {
            return EnvironmentLayer.NONE;
        }
    }

    public static EnvironmentLayer GetLayerByTag(uint Tag)
    {

        if (Tag == 0)
        {
            return EnvironmentLayer.GROUND;
        }
        else if (Tag == 1)
        {
            return EnvironmentLayer.OBSTACLE_HALF;
        }
        else if (Tag == 2)
        {
            return EnvironmentLayer.OBSTACLE_FULL;
        }
        else if (Tag == 3)
        {
            return EnvironmentLayer.WALL;
        }
        else if(Tag == 4)
        {
            return EnvironmentLayer.CHARACTER;
        }
        else
        {
            return EnvironmentLayer.NONE;
        }
    }

    public static int GetTraversibleTagMask()
    {
        return 1 << 0;
    }


    public static uint GetPathfindingTag(EnvironmentLayer layer)
    {
        switch (layer)
        {
            case EnvironmentLayer.GROUND:
                return 0;
            case EnvironmentLayer.OBSTACLE_HALF:
                return 1;
            case EnvironmentLayer.OBSTACLE_FULL:
                return 2;
            case EnvironmentLayer.WALL:
                return 3;
            case EnvironmentLayer.CHARACTER:
                return 4;
            case EnvironmentLayer.NONE:
                return 5;
        }

        return 0;
    }

    public static Vector3 GetTileSize()
    {
        return new Vector3(ENV_TILE_SIZE, 0, ENV_TILE_SIZE);
    }

    public static int CalculateGScore(int distance)
    {
        return distance * 1000;
    }
}