using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

[Serializable]
public struct TeamColorInfo
{
    public TeamID ID;
    public Color Color;
}

[Serializable]
public struct MovementRangeColorInfo
{
    public MovementRangeType ID;
    public Gradient Gradient;
    public Color Color;
}

[Serializable]
public struct CoverColorInfo
{
    public EnvironmentCover ID;
    public Color Color;
}

[Serializable]
public struct EnvironmentLayerColorInfo
{
    public EnvironmentLayer ID;
    public Color Color;
}

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Library/ColorLibrary", order = 1)]
public class ColorLibrary : ScriptableObject
{
    [Header("Teams")]
    [SerializeField] private List<TeamColorInfo> TeamColors;

    [Header("Movement Range")]
    [SerializeField] private List<MovementRangeColorInfo> MovementRangeColors;

    [Header("Cover")]
    [SerializeField] private List<CoverColorInfo> CoverColors;

    [Header("Layers")]
    [SerializeField] private List<EnvironmentLayerColorInfo> LayerColors;

    public Color Get(TeamID ID)
    {
        foreach (TeamColorInfo info in TeamColors)
        {
            if (info.ID == ID)
            {
                return info.Color;
            }
        }

        return Color.clear;
    }

    public Color Get(MovementRangeType ID)
    {
        foreach (MovementRangeColorInfo info in MovementRangeColors)
        {
            if (info.ID == ID)
            {
                return info.Color;
            }
        }

        return Color.clear;
    }

    public Color Get(EnvironmentCover ID)
    {
        foreach (CoverColorInfo info in CoverColors)
        {
            if (info.ID == ID)
            {
                return info.Color;
            }
        }

        return Color.clear;
    }

    public Color Get(bool flag)
    {
        if (flag)
        {
            return Color.green;
        }
        else
        {
            return Color.red;
        }
    }

    public Color Get(EnvironmentLayer ID)
    {
        foreach (EnvironmentLayerColorInfo info in LayerColors)
        {
            if (info.ID == ID)
            {
                return info.Color;
            }
        }

        return Color.clear;
    }
}
