using System;
using System.Collections.Generic;
using Constants;
using UnityEngine;

[Serializable]
public struct CharacterData
{
    [Header("Character Info")]
    public CharacterConstants.ClassID ClassID;
    [Space]
    public CharacterConstants.TypeID Type;
    [Space]
    public CharacterMarker Marker;
    [Space]

    [Space]
    public int Health;
}

[Serializable]
public struct SquadData
{
    [Header("Squad Data")]
    [Space]
    public CharacterConstants.TeamID ID;
    public List<CharacterData> Characters;
}

[Serializable]
public struct ArenaData
{
    [Header("Arena Data")]
    [Space]
    public List<SquadData> Squads;
    [Space]
    public CharacterConstants.TeamID PlayerTeam;
}