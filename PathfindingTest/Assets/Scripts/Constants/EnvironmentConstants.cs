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

    public enum EnvironmentLayer { None, Ground, Obstacle_Half, Obstacle_Full, Wall, Character };

    public enum EnvironmentDirection { NORTH, SOUTH, WEST, EAST, NORTH_EAST, NORTH_WEST, SOUTH_EAST, SOUTH_WEST }

    public enum EnvironmentCover { None, Half, Full };

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
        return (Layer == EnvironmentLayer.Obstacle_Full || Layer == EnvironmentLayer.Obstacle_Half) || Layer == EnvironmentLayer.Wall;
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
            case EnvironmentLayer.Wall:
            case EnvironmentLayer.Obstacle_Full:
            case EnvironmentLayer.Obstacle_Half:
            case EnvironmentLayer.None:
                return false;
            case EnvironmentLayer.Character:
                return true;
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
            case EnvironmentLayer.Ground:
                if (ShowTraversibles)
                {
                    return Color.green;
                }
                else
                {
                    return Color.clear;
                }
            case EnvironmentLayer.Obstacle_Full:
                return Color.red;
            case EnvironmentLayer.Obstacle_Half:
                return Color.yellow;
            case EnvironmentLayer.Character:
                return Color.cyan;
            case EnvironmentLayer.None:
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
            case EnvironmentCover.Half:
                return Color.yellow;
            case EnvironmentCover.Full:
                return Color.magenta;
            default:
                return Color.clear;
        }
    }

    public static float GetCoverHeight(EnvironmentCover cover)
    {
        switch(cover)
        {
            case EnvironmentCover.Half:
                return ENV_TILE_SIZE / 2;
            case EnvironmentCover.Full:
                return ENV_TILE_SIZE;
            default:
                return 0;
        }
    }

    public static EnvironmentCover GetCoverType(EnvironmentTileConnectionInfo info)
    {
        if (info.Layer == EnvironmentLayer.Obstacle_Half)
        {
            return EnvironmentCover.Half;
        }
        else if (info.Layer == EnvironmentLayer.Obstacle_Full || info.IsObstructed)
        {
            return EnvironmentCover.Full;
        }
        else
        {
            return EnvironmentCover.None;
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
            return EnvironmentLayer.Ground;
        }
        else if (Layer == LAYER_OBSTACLE_HALF)
        {
            return EnvironmentLayer.Obstacle_Half;
        }
        else if (Layer == LAYER_OBSTACLE_FULL)
        {
            return EnvironmentLayer.Obstacle_Full;
        }
        else if (Layer == LAYER_WALL)
        {
            return EnvironmentLayer.Wall;
        }
        else if (Layer == LAYER_CHARACTER)
        {
            return EnvironmentLayer.Character;
        }
        else
        {
            return EnvironmentLayer.None;
        }
    }

    public static EnvironmentLayer GetLayerByTag(uint Tag)
    {

        if (Tag == 0)
        {
            return EnvironmentLayer.Ground;
        }
        else if (Tag == 1)
        {
            return EnvironmentLayer.Obstacle_Half;
        }
        else if (Tag == 2)
        {
            return EnvironmentLayer.Obstacle_Full;
        }
        else if (Tag == 3)
        {
            return EnvironmentLayer.Wall;
        }
        else if(Tag == 4)
        {
            return EnvironmentLayer.Character;
        }
        else
        {
            return EnvironmentLayer.None;
        }
    }


    public static uint GetPathfindingTag(EnvironmentLayer layer)
    {
        switch (layer)
        {
            case EnvironmentLayer.Ground:
                return 0;
            case EnvironmentLayer.Obstacle_Half:
                return 1;
            case EnvironmentLayer.Obstacle_Full:
                return 2;
            case EnvironmentLayer.Wall:
                return 3;
            case EnvironmentLayer.Character:
                return 4;
            case EnvironmentLayer.None:
                return 5;
        }

        return 0;
    }
}