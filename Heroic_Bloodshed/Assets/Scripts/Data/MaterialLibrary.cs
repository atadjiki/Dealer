using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Library/MaterialLibrary", order = 1)]
public class MaterialLibrary : ScriptableObject
{
    [Header("Decals")]
    [SerializeField] private Material TileSelector;
    [SerializeField] private Material DestinationHighlight;
    [SerializeField] private Material PathDisplay;
    [SerializeField] private Material CharacterSelect;
    [SerializeField] private Material MovementRadius;

    public static Material Get(MaterialID ID)
    {
        Refresh();

        switch(ID)
        {
            case MaterialID.TILE_SELECTOR:
                return library.TileSelector;
            case MaterialID.DEST_HIGHLIGHT:
                return library.DestinationHighlight;
            case MaterialID.PATH_DISPLAY:
                return library.PathDisplay;
            case MaterialID.CHAR_SELECT:
                return library.CharacterSelect;
            case MaterialID.MOVE_RADIUS:
                return library.MovementRadius;
            default:
                return null;
        }
    }

    private static MaterialLibrary library;

    private static void Refresh()
    {
        if (library == null)
        {
            library = Load();
        }
    }

    public static MaterialLibrary Load()
    {
        Debug.Log("Loading MaterialLibrary");
        return Resources.Load<MaterialLibrary>("Data/Libraries/MaterialLibrary");
    }
}
