using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using static Constants;

public class EnvironmentDebugRandomPath : MonoBehaviour
{
    private CharacterComponent _character;
    private CharacterNavigator _navigator;
    private TileGraph _graph;

    private void Start()
    {
        _character = GetComponent<CharacterComponent>();

        _navigator = _character.GetNavigator();
        _navigator.DestinationReachedCallback += OnDestinationReached;

        _graph = (TileGraph)AstarPath.active.graphs[0];

        AttemptRandomPath();
    }

    private void OnDestroy()
    {
        _navigator.DestinationReachedCallback -= OnDestinationReached;
    }

    public void AttemptRandomPath()
    {
        StopAllCoroutines();
        StartCoroutine(Coroutine_AttemptRandomPath());
    }

    private IEnumerator Coroutine_AttemptRandomPath()
    {
        float idleTime = Random.Range(1.0f, 3f);
        Debug.Log("Waiting " + idleTime + " seconds...");

        yield return new WaitForSecondsRealtime(idleTime);

        GraphNode end = _graph.GetRandomNode();

        Vector3 destination = (Vector3) end.position;

        _character.SetActiveDestination(destination);

        yield return _character.Coroutine_PerformAbility(AbilityID.MOVE_FULL);

        yield return null;
    }

    private void OnDestinationReached(CharacterNavigator navigator)
    {
        Debug.Log("Attempting new random path!");

        AttemptRandomPath();
    }
}
