using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/EncounterPrefabData", order = 1)]
public class EncounterPrefabData : ScriptableObject
{
    public GameObject TileSelect;
    public GameObject MovementRadius;
    public GameObject CameraRig;
}
