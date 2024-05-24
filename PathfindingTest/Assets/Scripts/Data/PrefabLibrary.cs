using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Library/PrefabLibrary", order = 1)]
public class PrefabLibrary : ScriptableObject
{
    [Header("Encounter")]
    [Space]
    [Header("Camera Rig")]
    public GameObject CameraRig;

    private static PrefabLibrary library;

    public static PrefabLibrary Get()
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

    public static PrefabLibrary Load()
    {
        return Resources.Load<PrefabLibrary>("Data/Libraries/PrefabLibrary");
    }
}
