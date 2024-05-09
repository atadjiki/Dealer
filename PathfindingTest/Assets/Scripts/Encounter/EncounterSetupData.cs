using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/EncounterSetupData", order = 1)]
public class EncounterSetupData : ScriptableObject
{
    public CharacterID ToSpawn;
}
