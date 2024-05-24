using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

[Serializable]
public struct EnvironmentLayerColorInfo
{
    public EnvironmentLayer Layer;
    public Color Color;
}

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Library/ColorLibrary", order = 1)]
public class ColorLibrary : ScriptableObject
{
    [SerializeField] private List<EnvironmentLayerColorInfo> LayerColors;

    public Color GetColor(EnvironmentLayer Layer)
    {
        foreach (EnvironmentLayerColorInfo info in LayerColors)
        {
            if (info.Layer == Layer)
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
        return Resources.Load<ColorLibrary>("Data/Libraries/ColorLibrary");
    }
}
