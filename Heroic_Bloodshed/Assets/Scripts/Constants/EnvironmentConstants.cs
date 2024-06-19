using System;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public static partial class Constants
{
    public static float ENV_TILE_SIZE = 1.5f;
    public static int ENV_LVL_STEP = 2;
    public static int ENV_MAX_LVL = 10;

    public static int LAYER_DECAL = LayerMask.NameToLayer("Decal");
    public static int LAYER_GROUND = LayerMask.NameToLayer("GROUND");
    public static int LAYER_OBSTACLE_HALF = LayerMask.NameToLayer("OBSTACLE_HALF");
    public static int LAYER_OBSTACLE_FULL = LayerMask.NameToLayer("OBSTACLE_FULL");
    public static int LAYER_WALL_HALF = LayerMask.NameToLayer("WALL_HALF");
    public static int LAYER_WALL_FULL = LayerMask.NameToLayer("WALL_FULL");
    public static int LAYER_CHARACTER = LayerMask.NameToLayer("CHARACTER");
    public static int LAYER_STAIRS = LayerMask.NameToLayer("STAIRS");

    public static Vector3 GetNeighboringTileLocation(Vector3 origin, EnvironmentDirection dir)
    {
        Vector3 direction = GetDirectionVector(dir);

        return origin + direction;
    }

    public static Int3 GetNeighboringTileCoordinates(Vector3 origin, EnvironmentDirection dir)
    {
        Vector3 neighborOrigin = GetNeighboringTileLocation(origin, dir);

        return CalculateTileCoordinates(neighborOrigin);
    }

    public static Vector3 CalculateTileOrigin(int Row, int Level, int Column)
    {
        Vector3 origin = new Vector3(Row, Level * ENV_LVL_STEP, Column);

        origin *= ENV_TILE_SIZE;

        return origin + new Vector3(ENV_TILE_SIZE / 2, 0, ENV_TILE_SIZE / 2);
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

    //ROW, COL, LEVEL
    public static Int3 CalculateTileCoordinates(Vector3 origin)
    {
        origin -= new Vector3(ENV_TILE_SIZE / 2, 0, ENV_TILE_SIZE / 2);

        origin.x /= ENV_TILE_SIZE;
        origin.y /= ENV_TILE_SIZE * ENV_LVL_STEP;
        origin.z /= ENV_TILE_SIZE;

        return new Int3((int)origin.x, (int)origin.z, (int)origin.y);
    }

    public static bool IsLayerObstacle(EnvironmentLayer Layer)
    {
        switch(Layer)
        {
            case EnvironmentLayer.OBSTACLE_FULL:
            case EnvironmentLayer.OBSTACLE_HALF:
            case EnvironmentLayer.STAIRS:
            default:
                return false;
        }
    }

    public static bool IsLayerCover(EnvironmentLayer Layer)
    {
        switch(Layer)
        {
            case EnvironmentLayer.OBSTACLE_FULL:
            case EnvironmentLayer.OBSTACLE_HALF:
            case EnvironmentLayer.WALL_FULL:
            case EnvironmentLayer.WALL_HALF:
                return true;
            default:
                return false;
        }
    }

    public static uint GetDirectionCost(EnvironmentDirection Direction)
    {
        if(IsCardinalDirection(Direction))
        {
            return 1000;
        }
        else
        {
            return (uint) (Mathf.Sqrt(2)*1000);
        }
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

    public static bool IsLayerWalkable(EnvironmentLayer Layer)
    {
        switch (Layer)
        {
            case EnvironmentLayer.CHARACTER:
            case EnvironmentLayer.OBSTACLE_HALF:
            case EnvironmentLayer.OBSTACLE_FULL:
            case EnvironmentLayer.WALL_FULL:
            case EnvironmentLayer.WALL_HALF:
            case EnvironmentLayer.STAIRS:
            case EnvironmentLayer.NONE:
                return false;
            default:
                return true;
        }
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

    public static EnvironmentCover GetCoverType(TileConnectionInfo info)
    {
        if (info.Layer == EnvironmentLayer.OBSTACLE_HALF || info.Layer == EnvironmentLayer.WALL_HALF)
        {
            return EnvironmentCover.HALF;
        }
        else if (info.Layer == EnvironmentLayer.OBSTACLE_FULL || info.Layer == EnvironmentLayer.WALL_FULL || IsLayerCover(info.Obstruction))
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

    public static List<EnvironmentDirection> GetForwardBackwards()
    {
        return new List<EnvironmentDirection>()
        {
            EnvironmentDirection.NORTH,
            EnvironmentDirection.SOUTH,
        };
    }

    public static List<EnvironmentDirection> GetRightLeft()
    {
        return new List<EnvironmentDirection>()
        {
            EnvironmentDirection.EAST,
            EnvironmentDirection.WEST,
        };
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
        else if (Layer == LAYER_WALL_HALF)
        {
            return EnvironmentLayer.WALL_HALF;
        }
        else if (Layer == LAYER_WALL_FULL)
        {
            return EnvironmentLayer.WALL_FULL;
        }
        else if (Layer == LAYER_CHARACTER)
        {
            return EnvironmentLayer.CHARACTER;
        }
        else if (Layer == LAYER_STAIRS)
        {
            return EnvironmentLayer.STAIRS;
        }
        else
        {
            return EnvironmentLayer.NONE;
        }
    }

    public static int CalculateGScore(int range)
    {
        return range * 1000;
    }

    public static int GetElevation(float height)
    {
        float step = ENV_TILE_SIZE * ENV_LVL_STEP;

        for (int i = 0; i < ENV_MAX_LVL; i++)
        {
            float floor = i * step;
            float ceiling = (i + 1) * step;

            if(height >= floor && height < ceiling)
            {
                return i;
            }
        }

        return - 1;
    }
}