using System;
using System.Collections.Generic;
using Constants;

[Serializable]
public struct CharacterMarkerData
{
    public CharacterConstants.CharacterType type;
    public CharacterConstants.Weapon weapon;
}

[Serializable]
public struct TeamData
{
    public List<CharacterMarker> Markers;
}

[Serializable]
public struct ArenaData
{
    public CharacterConstants.Team ID;
    public List<TeamData> Teams;
}