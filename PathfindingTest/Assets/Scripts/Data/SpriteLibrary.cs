using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Library/SpriteLibrary", order = 1)]
public class SpriteLibrary : ScriptableObject
{
    private static SpriteLibrary library;

    public static SpriteLibrary Get()
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

    public static SpriteLibrary Load()
    {
        return Resources.Load<SpriteLibrary>("Data/Libraries/SpriteLibrary");
    }
}
