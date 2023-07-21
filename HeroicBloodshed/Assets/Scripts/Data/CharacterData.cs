using System;
using System.Collections.Generic;
using Constants;
using UnityEngine;

public class CharacterEncounterData
{
    public CharacterComponent Character;
    public int Health = 0;

    public CharacterEncounterData(CharacterComponent _Character, int _Health)
    {
        Character = _Character;
        Health = _Health;
    }
}

[Serializable]
public struct PlayerSpawnData
{
    [SerializeField] private CharacterConstants.PlayerID ID;
    public PlayerCharacterMarker Marker;

    public CharacterConstants.CharacterID GetID() { return CharacterConstants.ToCharacterID(ID); }
}

[Serializable]
public struct EnemySpawnData
{
    [SerializeField] private CharacterConstants.EnemyID ID;
    public EnemyCharacterMarker Marker;

    public CharacterConstants.CharacterID GetID() { return CharacterConstants.ToCharacterID(ID); }
}

[Serializable]
public struct EnemySpawnGroupData
{
    public List<EnemySpawnData> Enemies;
}