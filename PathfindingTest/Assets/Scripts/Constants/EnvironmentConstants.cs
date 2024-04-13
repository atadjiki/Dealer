using UnityEngine;

public static partial class Constants
{
    public static float TileSize = 1.5f;

    public static int Layer_Ground =        LayerMask.GetMask("EnvironmentGround");
    public static int Layer_Obstacle_Half = LayerMask.GetMask("EnvironmentObstacleHalf");
    public static int Layer_Obstacle_Full = LayerMask.GetMask("EnvironmentObstacleFull");

    public enum EnvironmentNodeState { Unwalkable, Walkable, Obstacle_Half, Obstacle_Full, Character };
}