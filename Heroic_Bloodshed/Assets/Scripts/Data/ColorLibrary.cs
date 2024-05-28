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

    public Color GetByID(TeamID ID)
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

    public Color GetColorByID(MovementRangeType ID)
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

    public Gradient GetGradientyID(MovementRangeType ID)
    {
        foreach(MovementRangeColorInfo info in MovementRangeColors)
        {
            if(info.ID == ID)
            {
                return info.Gradient;
            }
        }

        return null;
    }

    public Color GetByID(EnvironmentCover ID)
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

    public Color GetByState(bool flag)
    {
        if(flag)
        {
            return Color.green;
        }
        else
        {
            return Color.red;
        }
    }

    public Color GetByID(EnvironmentLayer ID)
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

    private static ColorLibrary library;

    public static ColorLibrary Get()
    {
        if(library != null)
        {
            return library;
        }
        else
        {
            library = Load();
            return library;
        }
    }

    public static ColorLibrary Load()
    {
        Debug.Log("Loading ColorLibrary");
        return Resources.Load<ColorLibrary>("Data/Libraries/ColorLibrary");
    }
}
