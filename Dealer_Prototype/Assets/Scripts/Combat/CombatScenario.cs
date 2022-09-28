using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine;

public class CombatScenario : MonoBehaviour
{
    [Header("Data")]
    [Header("Controllers")]
    public Enumerations.ControllerType Controller_Defending;
    public Enumerations.ControllerType Controller_Opposing;
    [Header("Rosters")]
    public Roster Roster_Defending;
    public Roster Roster_Opposing;
    [Header("Arena")]
    public Arena Arena;

    [Header("State")]
    public Enumerations.ScenarioState State;

    private void Start()
    {
        Arena.Setup(Roster_Defending, Roster_Opposing);
    }

    //helper
    public int GetDefendingCount()
    {
        return Roster_Defending.GetSize();
    }

    public int GetOpposingCount()
    {
        return Roster_Opposing.GetSize();
    }
}
