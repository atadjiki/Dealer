using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

public class EnvironmentUtil
{
    public static EnvironmentTileState ScanForHit(Vector3 center)
    {
        float height = 3;

        Vector3 heightOffset = new Vector3(0, height, 0);

        Vector3 rayOrigin = center + heightOffset;

        Ray ray = new Ray(rayOrigin, Vector3.down);
        RaycastHit hitInfo;

        Debug.DrawRay(rayOrigin, Vector3.down, Color.blue, Time.deltaTime, false);

        if (Physics.Raycast(ray, out hitInfo, height*1.5f))
        {
            if(hitInfo.collider != null)
            {
                int layer = hitInfo.collider.gameObject.layer;

                EnvironmentTileState state = GetTileState(layer);

                Debug.DrawRay(center, Vector3.up, GetTileStateColor(state), Time.deltaTime, false);

                return state;
            }
        }

        return EnvironmentTileState.None;
    }

    public static Vector3 CalculateTileCenter(int row, int column)
    {
        Vector3 basePosition = new Vector3(row * ENV_TILE_SIZE, 0, column * ENV_TILE_SIZE);

        return basePosition + new Vector3(ENV_TILE_SIZE / 2, 0, ENV_TILE_SIZE / 2);
    }

    public static EnvironmentTileState GetTileState(int layer)
    {
        if (layer == LAYER_GROUND)
        {
            return EnvironmentTileState.Ground;
        }
        else if(layer == LAYER_OBSTACLE_HALF)
        {
            return EnvironmentTileState.Obstacle_Half;
        }
        else if(layer == LAYER_OBSTACLE_FULL)
        {
            return EnvironmentTileState.Obstacle_Full;
        }

        return EnvironmentTileState.None;
    }

    public static Color GetTileStateColor(EnvironmentTileState State)
    {
        switch (State)
        {
            case EnvironmentTileState.Ground:
                return Color.green;
            case EnvironmentTileState.Obstacle_Full:
            case EnvironmentTileState.Obstacle_Half:
                return Color.yellow;
            case EnvironmentTileState.None:
                return Color.red;
            default:
                return Color.clear;
        }
    }
}
