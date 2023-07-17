using System;
using System.Collections.Generic;
using Constants;
using UnityEngine;

[Serializable]
public struct CharacterData
{
    [Header("Character Info")]
    [Space]
    public CharacterConstants.CharacterType type;
    public CharacterConstants.Weapon weapon;
    [Space]
    public CharacterMarker marker;
    [Space]

    [Space]
    public int health;
}

[Serializable]
public struct SquadData
{
    [Header("Squad Data")]
    [Space]
    public CharacterConstants.Team ID;
    public List<CharacterData> Characters;
}

[Serializable]
public struct ArenaData
{
    [Header("Arena Data")]
    [Space]
    public List<SquadData> Squads;
    [Space]
    public CharacterConstants.Team PlayerTeam;
}