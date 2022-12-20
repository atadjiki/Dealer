using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using Pathfinding.RVO;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(Seeker))]
[RequireComponent(typeof(AIPath))]

public class NavigatorComponent : MonoBehaviour, IGameplayInitializer
{
    //pathfinding AI
    private Seeker _seeker;
    private AIPath _AI;

    private bool _initialized = false;

    public bool HasInitialized()
    {
        return _initialized;
    }

    public void Initialize()
    {
        StartCoroutine(PerformInitialize());
    }

    public IEnumerator PerformInitialize()
    {
        _seeker = GetComponent<Seeker>();

        yield return new WaitUntil(() => _seeker != null);

        _AI = GetComponent<AIPath>();

        yield return new WaitUntil(() => _AI != null);

        AstarPath.active.ScanAsync();

        _initialized = true;
    }

    public void SetDestination(Vector3 destination)
    {
        _AI.destination = destination;
        _AI.SearchPath();
    }

    public Vector3 GetDestination()
    {
        return _AI.destination;
    }

    public float GetDistanceToDestination()
    {
        return _AI.remainingDistance;
    }

    public Vector3 GetStartOfPath()
    {
        if(_AI.hasPath)
        {
            Path path = _seeker.GetCurrentPath();

            if(path.vectorPath.Count > 0)
            {
                return path.vectorPath[0];
            }
        }

        return this.transform.position;
    }

    public Vector3 GetNextPointInPath()
    {
        if (_AI.hasPath)
        {
            Path path = _seeker.GetCurrentPath();

            if (path.vectorPath.Count > 1)
            {
                return path.vectorPath[1];
            }
        }

        return GetStartOfPath();
    }

    public void ToggleMovement(bool flag)
    {
        _AI.canMove = flag;
    }

    public bool IsMoving()
    {
        return !_AI.isStopped && !_AI.reachedEndOfPath;
    }
}
