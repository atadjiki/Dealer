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
    public List<CharacterMarker> Markers; //where are they spawning?
}

public class EncounterSetupData : MonoBehaviour
{
    public List<EncounterTeamData> Teams;
    public CinemachineVirtualCamera VirtualCamera;
}
