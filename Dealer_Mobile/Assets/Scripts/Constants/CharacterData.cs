using System;
using System.Collections.Generic;
using Constants;
using UnityEngine;

[Serializable]
public struct PlayerSpawnData
{
    public CharacterConstants.PlayerID ID;
    public PlayerCharacterMarker Marker;
}

[Serializable]
public struct EnemySpawnData
{
    public CharacterConstants.EnemyID ID;
    public EnemyCharacterMarker Marker;
}

[Serializable]
public struct WaveData
{
    public List<EnemySpawnData> Enemies;
}

[Serializable]
public struct EncounterData
{
    public List<PlayerSpawnData> PlayerSquad;
    public List<WaveData> Waves;
}