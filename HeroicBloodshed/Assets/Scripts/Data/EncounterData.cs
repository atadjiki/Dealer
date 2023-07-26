using System;
using System.Collections.Generic;
using Constants;
using UnityEngine;

public class EncounterStateData
{
    //the initial state of players and enemies
    public EncounterSetupData SetupData;

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
