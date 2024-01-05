using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static partial class Constants
{
    public static float TILE_SIZE = 1.5f; //if you change this, the game will explode

    public static uint TAG_LAYER_GROUND = 0;
    public static uint TAG_LAYER_OBSTACLE = 1;
    public static uint TAG_LAYER_CHARACTER = 2;
    public static uint TAG_LAYER_SPAWNLOCATION = 3;
    public static uint TAG_LAYER_WALL = 4;
    public static uint TAG_LAYER_COVER = 5;

    public static Vector3 GetTileScaleVector()
    {
        return new Vector3(TILE_SIZE, TILE_SIZE, TILE_SIZE);
    }

    public static int MAX_SEARCH_DISTANCE = 36; //1.5 * 6 * 2
}
