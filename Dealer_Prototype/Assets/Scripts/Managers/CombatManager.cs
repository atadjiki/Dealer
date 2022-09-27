using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine;

public class CombatManager : Singleton<CombatManager>
{
    private List<CombatScenario> scenarios;

    protected override void Awake()
    {
        base.Awake();

        scenarios = new List<CombatScenario>();
    }

    public void RegisterScenario(CombatScenario scenario)
    {
        scenarios.Add(scenario);

        Debug.Log("Registered scenario " + scenario.name);

        AdvanceScenario(scenario);
    }

    private void AdvanceScenario(CombatScenario scenario)
    {
        switch(scenario.State)
        {
            case Enumerations.ScenarioState.Null:
                InitializeScenario(scenario);
                break;

            default:
                break;
        }
    }

    private void InitializeScenario(CombatScenario scenario)
    {
        scenario.Info.Arena.SpawnDefendingTeam(scenario.Info.Team_Defending);
        scenario.Info.Arena.SpawnOpposingTeam(scenario.Info.Team_Opposing);

        scenario.State = Enumerations.ScenarioState.Initialized;

        Debug.Log("scenario " + scenario.name + " - " + scenario.State);
    }

}
