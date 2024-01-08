using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static partial class Constants
{
    public static float TILE_SIZE = 1.5f; //if you change this, the game will explode

    //if you change these, the game will explode
    //make sure they match the tag settings in the Astar pathfinder menu!
    public static uint TAG_LAYER_GROUND = 0;
    public static uint TAG_LAYER_OBSTACLE = 1;
    public static uint TAG_LAYER_COVER = 2;
    public static uint TAG_LAYER_WALL_DEFAULT = 3;
    public static uint TAG_LAYER_WALL_SPAWN = 4;
    public static uint TAG_LAYER_CHARACTER = 5;

    public static int MAX_SEARCH_DISTANCE = 36; //1.5 * 6 * 2
}
