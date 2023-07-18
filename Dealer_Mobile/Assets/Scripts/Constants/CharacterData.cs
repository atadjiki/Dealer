using System;
using System.Collections.Generic;
using Constants;
using UnityEngine;

[Serializable]
public struct CharacterDefinition
{
    public CharacterConstants.UniqueID ID;

    public List<CharacterConstants.ModelID> AllowedModels;

    public List<CharacterConstants.WeaponID> AllowedWeapons;

    public int BaseHealth;
}

[Serializable]
public struct CharacterSpawnData
{
    //who are we spawning and where?
    [Header("Character Info")]
    public CharacterConstants.UniqueID ID;
    [Space]
    public CharacterMarker Marker;
}

[Serializable]
public struct WaveData
{
    //what enemies are we spawning?
    [Header("Wave Data")]
    [Space]
    public List<CharacterSpawnData> Characters;
}

[Serializable]
public struct EncounterData
{
    //how many waves in encounter?
    [Header("Encounter Data")]
    [Space]
    public List<WaveData> Waves;
}