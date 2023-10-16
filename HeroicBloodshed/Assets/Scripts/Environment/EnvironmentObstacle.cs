using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

public class EnvironmentObstacle : MonoBehaviour
{
    [SerializeField] private EnvironmentObstacleType ObstacleType;

    public EnvironmentObstacleType GetObstacleType() { return ObstacleType; }
}
