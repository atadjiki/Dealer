using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

public static partial class Constants
{
    public static Color Col_Yellow = new Color(230, 145, 69);
    public static Color Col_Red = new Color(223,22,57);
    public static Color Col_Red_Dark = new Color(138,38,58);
    public static Color Col_Blue = new Color(181,323,240);


    public static Color GetColor(MovementRangeType rangeType)
    {
        switch(rangeType)
        {
            case MovementRangeType.Full:
                return Col_Yellow;
            case MovementRangeType.Half:
                return Col_Blue;
            default:
                return Color.grey;
        }
    }
    
}
