using System;
using System.Collections.Generic;
using Constants;
using UnityEngine;

public class EncounterStateData
{
    //the initial set of players and enemies
    public List<CharacterEncounterData> PlayerCharacters = new List<CharacterEncounterData>();
    public List<CharacterEncounterData> EnemyCharacters = new List<CharacterEncounterData>();

    //queues are built each turn based on who is still alive
    public Queue<CharacterEncounterData> PlayerQueue = null;
    public Queue<CharacterEncounterData> EnemyQueue = null;

    public int TurnCount = 0;
    public CharacterConstants.TeamID CurrentTurn = CharacterConstants.TeamID.Player;

    private bool _initialized;

    public void Initialized() { _initialized = true; }
    public bool IsInitialized() { return _initialized; }
}

[Serializable]
public struct EncounterSetupData
{
    public List<PlayerSpawnData> PlayerCharacters;
    public List<EnemySpawnGroupData> EnemyGroups;
}
