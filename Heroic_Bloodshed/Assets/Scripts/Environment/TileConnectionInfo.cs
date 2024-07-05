using System;
using Pathfinding;
using static Constants;

[Serializable]
public struct TileConnectionInfo
{
    public TileNode Node;
    public EnvironmentLayer Layer;
    public EnvironmentLayer Obstruction;

    public static TileConnectionInfo Build()
    {
        return new TileConnectionInfo()
        {
            Node = null,
            Layer = EnvironmentLayer.NONE,
            Obstruction = EnvironmentLayer.NONE,
        };
    }

    public bool IsWallBetween()
    {
        return (Obstruction == EnvironmentLayer.WALL_HALF || Obstruction == EnvironmentLayer.WALL_FULL);
    }

    public bool IsLayerBetween(EnvironmentLayer layer)
    {
        return Obstruction == layer;
    }

    public bool IsObstructed()
    {
        return Obstruction != EnvironmentLayer.NONE;
    }

    public bool IsUnobstructed()
    {
        return (IsObstructed() == false);
    }

    public bool IsValid()
    {
        return IsLayerWalkable(Layer) && Obstruction == EnvironmentLayer.NONE;
    }

    public bool IsInvalid()
    {
        return (IsValid() == false);
    }

    public bool ProvidesCover()
    {
        return IsLayerObstacle(Layer) && IsLayerCover(Obstruction);
    }
}