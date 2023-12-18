using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

public static partial class Constants
{
    public static Color Player_Default = new Color(0.44706f,  0.60000f,  0.66275f);
    public static Color Enemy_Default  = new Color(0.68235f,  0.15294f,  0.17255f);

    public static Color Movement_Half = new Color(0.314f, 0.776f, 0.969f);

    public static Color Movement_Full = new Color(0.98f, 0.808f, 0.282f);

    public static Color Movement_None = new Color(0.53333f,  0.53333f,  0.53333f);

    public static Color GetColor(MovementRangeType rangeType)
    {
        switch (rangeType)
        {
            case MovementRangeType.Full:
                return Movement_Full;
            case MovementRangeType.Half:
                return Movement_Half;
            default:
                return Movement_None;
        }
    }

    public static Color GetColor(TeamID teamID, float opacity)
    {
        Color teamColor;

        switch (teamID)
        {
            case TeamID.Player:
                teamColor = Player_Default;
                break;
            case TeamID.Enemy:
                teamColor = Enemy_Default;
                break;
            default:
                teamColor = Color.clear;
                break;
        }

        teamColor.a = opacity;

        return teamColor;
    }

}
