using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

public class EnvironmentSpawnMarker : EnvironmentMarker
{
    [SerializeField] private TeamID Team;

    public TeamID GetTeam() { return Team; }
}
