using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine;

[System.Serializable]
public class CombatInfo
{
    [Header("Controllers")]
    public Enumerations.ControllerType Controller_Defending;
    public Enumerations.ControllerType Controller_Opposing;
    [Header("Rosters")]
    public Roster Team_Defending;
    public Roster Team_Opposing;
    [Header("Arena")]
    public CombatArena Arena;
}

public class CombatScenario : MonoBehaviour
{
    [Header("Data")]
    public CombatInfo Info;

    [Header("State")]
    public Enumerations.ScenarioState State;

    private void Start()
    {
        CombatManager.Instance.RegisterScenario(this);
    }
}


