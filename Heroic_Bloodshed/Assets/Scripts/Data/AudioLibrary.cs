using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Library/AudioLibrary", order = 1)]
public class AudioLibrary : ScriptableObject
{
    private static AudioLibrary library;

    private static void Refresh()
    {
        if (library == null)
        {
            library = Load();
        }
    }

    public static AudioLibrary Load()
    {
        Debug.Log("Loading AudioLibrary");
        return Resources.Load<AudioLibrary>("Data/Libraries/AudioLibrary");
    }
}
