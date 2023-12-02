using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

public struct EnvironmentInputData
{
    public bool OnValidTile;
    public Vector3 TilePosition;
    public MovementRangeType RangeType;
    public int PathCost;
    public List<Vector3> VectorPath;

    public static EnvironmentInputData Build()
    {
        return new EnvironmentInputData()
        {
            OnValidTile = false,
            TilePosition = Vector3.zero,
            RangeType = MovementRangeType.None,
            PathCost = -1,

            VectorPath = new List<Vector3>(),
        };
    }

    public override string ToString()
    {
        string result = "Input Data:\n";

        result += "Valid Tile " + OnValidTile + "\n";
        result += "Position "   + TilePosition + "\n";
        result += "Range Type"  + RangeType+ "\n";
        result += "Path Cost "  + PathCost+ "\n";

        return result;
    }
}

public interface IEnvironmentInputHandler 
{
    public IEnumerator PerformInputUpdate(EnvironmentInputData InputData);
}
