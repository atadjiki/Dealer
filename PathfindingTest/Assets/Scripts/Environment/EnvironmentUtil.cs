using System.Collections;
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

        //Debug.DrawRay(origin, direction, Color.blue, Time.deltaTime, false);

        if (Physics.Raycast(ray, out hitInfo, range))
        {
            if(hitInfo.collider != null)
            {
                int layer = hitInfo.collider.gameObject.layer;

                EnvironmentLayer state = GetLayer(layer);

               // Debug.DrawRay(hitInfo.point, Vector3.up, GetLayerDebugColor(state), Time.deltaTime, false);

                return state;
            }
        }

        return EnvironmentLayer.None;
    }

    public static Vector3 CalculateTileOrigin(int Row, int Column)
    {
        Vector3 tilePivot = new Vector3(Row * ENV_TILE_SIZE, 0, Column * ENV_TILE_SIZE);

        return tilePivot + new Vector3(ENV_TILE_SIZE / 2, 0, ENV_TILE_SIZE / 2);
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

    public static Color GetLayerDebugColor(EnvironmentLayer Layer)
    {
        switch (Layer)
        {
            case EnvironmentLayer.Ground:
                return Color.green;
            case EnvironmentLayer.Obstacle_Full:
            case EnvironmentLayer.Obstacle_Half:
                return Color.yellow;
            case EnvironmentLayer.None:
                return Color.red;
            default:
                return Color.clear;
        }
    }
}
