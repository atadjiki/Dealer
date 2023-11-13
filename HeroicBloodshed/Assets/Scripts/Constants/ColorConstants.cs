using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

public static partial class Constants
{
    public static Color GetColor(MovementRangeType rangeType)
    {
        switch(rangeType)
        {
            case MovementRangeType.Full:
                return Color.yellow;
            case MovementRangeType.Half:
                return Color.blue;
            default:
                return Color.clear;
        }
    }
    
}
