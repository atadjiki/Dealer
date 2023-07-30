using System;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

[Serializable]
public struct EncounterTeamData
{
    public TeamID Team;
    public List<CharacterID> Characters; //who are we spawning?
    public List<CharacterMarker> Markers; //where are they spawning?
}


