using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

public static partial class Constants
{
    public static Color Col_Blue_Light = new Color(0.45098f,  0.83529f,  0.83529f);

    public static Color Col_Yellow_Light = new Color(0.98431f,  0.84706f,  0.47843f);

    public static Color Col_Grey= new Color(0.53333f,  0.53333f,  0.53333f);

    public static Color GetColor(MovementRangeType rangeType)
    {
        switch (rangeType)
        {
            case MovementRangeType.Full:
                return Col_Yellow_Light;
            case MovementRangeType.Half:
                return Col_Blue_Light;
            default:
                return Col_Grey;
        }
    }

}
