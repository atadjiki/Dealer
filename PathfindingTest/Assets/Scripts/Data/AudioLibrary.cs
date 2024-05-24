using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Library/AudioLibrary", order = 1)]
public class AudioLibrary : ScriptableObject
{
    private static AudioLibrary library;

    public static AudioLibrary Get()
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

    public static AudioLibrary Load()
    {
        return Resources.Load<AudioLibrary>("Data/Libraries/AudioLibrary");
    }
}
