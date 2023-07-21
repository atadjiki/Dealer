using System;
using System.Collections.Generic;
using Constants;
using UnityEngine;

public struct PlayerCombatData
{
    //player model
    //marker
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