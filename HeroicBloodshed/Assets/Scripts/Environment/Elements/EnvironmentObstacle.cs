using EPOOutline;
using Pathfinding;
using Pathfinding.RVO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static Constants;

[RequireComponent(typeof(BoxCollider))]

public class EnvironmentObstacle : MonoBehaviour
{
    [SerializeField] private EnvironmentObstacleType ObstacleType;

    public EnvironmentObstacleType GetObstacleType() { return ObstacleType; }

    private BoxCollider _collider;

    private void Awake()
    {
        _collider = GetComponent<BoxCollider>();
    }
    
    public Bounds GetBounds()
    {
        return _collider.bounds;
    }
}
