using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine;

[System.Serializable]
public class CombatInfo
{
    public Roster Team_Defending;
    public Roster Team_Opposing;

    public List<Roster> GetRosters()
    {
        return new List<Roster>() { Team_Defending, Team_Opposing };
    }

    public Roster GetRosterByTeam(Enumerations.Team team)
    {
        foreach (Roster roster in GetRosters())
        {
            if (roster.Team == team)
            {
                return roster;
            }
        }

        return null;
    }

}

public class CombatScenario : MonoBehaviour
{
    [Header("Data")]
    public CombatInfo Info;
    [Header("Arena")]
    public CombatArena Arena;

    private void Start()
    {
        Arena.SpawnDefendingTeam(Info.Team_Defending);
        Arena.SpawnOpposingTeam(Info.Team_Opposing);
    }
}
