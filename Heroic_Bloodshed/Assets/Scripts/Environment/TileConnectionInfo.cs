using System;
using static Constants;

[Serializable]
public struct TileConnectionInfo
{
    public EnvironmentLayer Layer;
    public EnvironmentLayer Obstruction;

    public static TileConnectionInfo Build()
    {
        return new TileConnectionInfo()
        {
            Layer = EnvironmentLayer.NONE,
            Obstruction = EnvironmentLayer.NONE,
        };
    }

    public bool IsWallBetween()
    {
        return (Obstruction == EnvironmentLayer.WALL_HALF || Obstruction == EnvironmentLayer.WALL_FULL);
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
        return IsLayerTraversible(Layer) && Obstruction == EnvironmentLayer.NONE;
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