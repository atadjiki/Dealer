using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : Singleton<CombatManager>
{
    public Scenario scenario;
    public Arena arena;

    protected override void Start()
    {
        base.Start();

        arena.Setup(scenario.Roster_Defending, scenario.Roster_Opposing);
    }

}
