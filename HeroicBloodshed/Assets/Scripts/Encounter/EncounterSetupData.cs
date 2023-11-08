using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

using static Constants;

[Serializable]
public struct EncounterTeamData
{
    public TeamID Team;
    public List<CharacterID> Characters; //who are we spawning?
}

public class EncounterSetupData : MonoBehaviour
{
    public bool IsPlayerCPU;
    public List<EncounterTeamData> Teams;
}
