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

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/ColorPalette", order = 1)]
public class ColorPalette : ScriptableObject
{
    private static ColorPalette _palette;

    public ColorPalette Get()
    {
        if(_palette != null)
        {
            return _palette;
        }
        else
        {
            _palette = ResourceUtil.GetColorPalette();
            return _palette;
        }
    }

    [SerializeField] private List<EnvironmentLayerColorInfo> LayerColors;

    public Color GetColor(EnvironmentLayer Layer)
    {
        foreach(EnvironmentLayerColorInfo info in LayerColors)
        {
            if(info.Layer == Layer)
            {
                return info.Color;
            }
        }

        return Color.clear;
    }
}
