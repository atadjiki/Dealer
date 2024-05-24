using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Library/MaterialLibrary", order = 1)]
public class MaterialLibrary : ScriptableObject
{
    [Header("Decals")]
    public Material TileSelector;
    public Material DestinationHighlight;
    public Material PathDisplay;
    public Material CharacterSelect;
    public Material MovementRadius;

    private static MaterialLibrary library;

    public static MaterialLibrary Get()
    {
        if (library != null)
        {
            return library;
        }
        else
        {
            library = Load();
            return library;
        }
    }

    public static MaterialLibrary Load()
    {
        return Resources.Load<MaterialLibrary>("Data/Libraries/MaterialLibrary");
    }
}
