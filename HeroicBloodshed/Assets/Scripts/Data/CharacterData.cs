using System;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

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
    [SerializeField] private PlayerID ID;
    public PlayerCharacterMarker Marker;

    public CharacterID GetID() { return ToCharacterID(ID); }
}

[Serializable]
public struct EnemySpawnData
{
    [SerializeField] private EnemyID ID;
    public EnemyCharacterMarker Marker;

    public CharacterID GetID() { return ToCharacterID(ID); }
}

[Serializable]
public struct EnemySpawnGroupData
{
    public List<EnemySpawnData> Enemies;
}