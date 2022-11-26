using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

[RequireComponent(typeof(Seeker))]
[RequireComponent(typeof(AIPath))]
public class NavigatorComponent : RunTimeComponent
{
    private Seeker _seeker;
    private AIPath _AI;

    protected override IEnumerator PerformInitialize()
    {
        _seeker = GetComponent<Seeker>();

        yield return new WaitUntil(() => _seeker != null);

        _AI = GetComponent<AIPath>();

        yield return new WaitUntil(() => _AI != null);

        yield return base.PerformInitialize();
    }
}
