using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

public class EnvironmentSpawnPoint : MonoBehaviour
{
    [SerializeField] private TeamID Team;

    [SerializeField] private Transform SpawnTransform;

    public TeamID GetTeam() { return Team; }

    public virtual void Activate() { }

    public virtual Transform GetSpawnTransform()
    {
        if(SpawnTransform == null)
        {
            return this.transform;
        }

        return SpawnTransform;
    }
}
