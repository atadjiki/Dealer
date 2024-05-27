using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

public class EnvironmentSpawnPoint : MonoBehaviour
{
    [SerializeField] private TeamID Team;

    public TeamID GetTeam() { return Team; }

    public virtual void Activate() { }
}
