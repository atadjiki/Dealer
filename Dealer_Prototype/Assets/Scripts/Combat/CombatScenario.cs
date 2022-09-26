using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine;

[System.Serializable]
public class CombatInfo
{
    public Roster Team_A;
    public Roster Team_B;

    public List<Roster> GetRosters()
    {
        return new List<Roster>() { Team_A, Team_B };
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
        Arena.SpawnTeams(Info.Team_A, Info.Team_B);
    }
}
