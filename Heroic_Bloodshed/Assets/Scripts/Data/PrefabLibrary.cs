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

    public static GameObject GetCameraRig()
    {
        Refresh();

        return library.CameraRig;
    }

    private static void Refresh()
    {
        if (library == null)
        {
            library = Load();
        }
    }

    public static PrefabLibrary Load()
    {
        Debug.Log("Loading PrefabLibrary");
        return Resources.Load<PrefabLibrary>("Data/Libraries/PrefabLibrary");
    }
}
