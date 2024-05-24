using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HighlightPlus;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Library/HighlightProfileLibrary", order = 1)]
public class HighlightProfileLibrary : ScriptableObject
{
    public HighlightProfile MovementRadius;

    private static HighlightProfileLibrary library;

    public static HighlightProfileLibrary Get()
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

    public static HighlightProfileLibrary Load()
    {
        Debug.Log("Loading HighlightProfileLibrary");
        return Resources.Load<HighlightProfileLibrary>("Data/Libraries/HighlightProfileLibrary");
    }
}
