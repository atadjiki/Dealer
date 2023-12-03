using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

public struct EnvironmentInputData
{
    public bool OnValidTile;
    public Vector3 NodePosition;
    public MovementRangeType RangeType;
    public int PathCost;
    public List<Vector3> PathToHighlightedNode;
    public Dictionary<Vector3, int> RadiusMap;

    public static EnvironmentInputData Build()
    {
        return new EnvironmentInputData()
        {
            OnValidTile = false,
            NodePosition = Vector3.zero,
            RangeType = MovementRangeType.None,
            PathCost = -1,

            PathToHighlightedNode = new List<Vector3>(),

            RadiusMap = new Dictionary<Vector3, int>(),
        };
    }

    public override string ToString()
    {
        string result = "Input Data:\n";

        result += "Valid Tile " + OnValidTile + "\n";
        result += "Position "   + NodePosition + "\n";
        result += "Range Type"  + RangeType+ "\n";
        result += "Path Cost "  + PathCost+ "\n";

        return result;
    }
}

public class EnvironmentInputHandler : MonoBehaviour
{ 
    public virtual IEnumerator PerformInputUpdate(EnvironmentInputData InputData)
    {
        yield return null;
    }

    public virtual void Activate()
    {
    }

    public virtual void Deactivate()
    {
        StopAllCoroutines();
    }
}
