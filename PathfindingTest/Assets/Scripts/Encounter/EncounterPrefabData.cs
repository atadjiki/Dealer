using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/EncounterPrefabData", order = 1)]
public class EncounterPrefabData : ScriptableObject
{
    [Header("Camera Rig")]
    public GameObject CameraRig;

    [Header("Environment Decals")]
    public GameObject TileSelector;
    public GameObject MovementRadius;
    public GameObject PathDisplay;
}
