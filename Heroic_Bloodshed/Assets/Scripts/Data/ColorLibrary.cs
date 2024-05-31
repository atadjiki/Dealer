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

    public static Color Get(TeamID ID)
    {
        Refresh();

        foreach (TeamColorInfo info in library.TeamColors)
        {
            if (info.ID == ID)
            {
                return info.Color;
            }
        }

        return Color.clear;
    }

    public static Color Get(MovementRangeType ID)
    {
        Refresh();

        foreach (MovementRangeColorInfo info in library.MovementRangeColors)
        {
            if (info.ID == ID)
            {
                return info.Color;
            }
        }

        return Color.clear;
    }

    public static Color Get(EnvironmentCover ID)
    {
        Refresh();

        foreach (CoverColorInfo info in library.CoverColors)
        {
            if (info.ID == ID)
            {
                return info.Color;
            }
        }

        return Color.clear;
    }

    public static Color Get(bool flag)
    {
        Refresh();

        if (flag)
        {
            return Color.green;
        }
        else
        {
            return Color.red;
        }
    }

    public static Color Get(EnvironmentLayer ID)
    {
        Refresh();

        foreach (EnvironmentLayerColorInfo info in library.LayerColors)
        {
            if (info.ID == ID)
            {
                return info.Color;
            }
        }

        return Color.clear;
    }

    private static ColorLibrary library;

    private static void Refresh()
    {
        if(library == null)
        {
            library = Load();
        }
    }

    public static ColorLibrary Load()
    {
        Debug.Log("Loading ColorLibrary");
        return Resources.Load<ColorLibrary>("Data/Libraries/ColorLibrary");
    }
}
