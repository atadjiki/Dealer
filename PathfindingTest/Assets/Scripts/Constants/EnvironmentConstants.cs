using UnityEngine;

public static partial class Constants
{
    public static float ENV_TILE_SIZE = 1.5f;

    public static int LAYER_GROUND = LayerMask.NameToLayer("GROUND");
    public static int LAYER_OBSTACLE_HALF = LayerMask.NameToLayer("OBSTACLE_HALF");
    public static int LAYER_OBSTACLE_FULL = LayerMask.NameToLayer("OBSTACLE_FULL");
    public static int LAYER_WALL = LayerMask.NameToLayer("WALL");

    public enum EnvironmentLayer { None, Ground, Obstacle_Half, Obstacle_Full, Wall };

    public enum EnvironmentDirection { NORTH, SOUTH, WEST, EAST, NORTH_EAST, NORTH_WEST, SOUTH_EAST, SOUTH_WEST }
}