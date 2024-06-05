using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

[Serializable]
public struct EncounterTeamData
{
    public TeamID Team;
    public List<CharacterID> Characters; //who are we spawning?
}

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/EncounterSetupData", order = 1)]
public class EncounterSetupData : ScriptableObject
{
    public List<EncounterTeamData> Teams;
}
