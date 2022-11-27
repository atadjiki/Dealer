using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

[RequireComponent(typeof(Seeker))]
[RequireComponent(typeof(AIPath))]
public class NavigatorComponent : MonoBehaviour, IGameplayInitializer
{
    private Seeker _seeker;
    private AIPath _AI;

    private float radius = 5;

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

        _initialized = true;
    }

    //navigator stuff

    Vector3 PickRandomPoint()
    {
        var point = Random.insideUnitSphere * radius;

        point.y = 0;
        point += _AI.position;
        return point;
    }

    void Update()
    {
        if (!_initialized) return;

        // Update the destination of the AI if
        // the AI is not already calculating a path and
        // the ai has reached the end of the path or it has no path at all
        if (!_AI.pathPending && (_AI.reachedEndOfPath || !_AI.hasPath))
        {
            _AI.destination = PickRandomPoint();
            _AI.SearchPath();
        }
    }
}
