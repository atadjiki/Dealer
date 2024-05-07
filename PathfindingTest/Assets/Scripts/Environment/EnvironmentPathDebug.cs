using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnvironmentPathDebug : MonoBehaviour
{
    [SerializeField] private Transform Start;
    [SerializeField] private Transform End;

    private Seeker _seeker;
    private AIPath _AI;

    private TileGraph _graph;

    private void Awake()
    {
        _seeker = GetComponentInChildren<Seeker>();

        _AI = GetComponentInChildren<AIPath>();
        _AI.canSearch = false;

        _graph = (TileGraph) AstarPath.active.graphs[0];

        StartCoroutine(Coroutine_AttemptPath(Start.position, End.position));
    }

    private IEnumerator Coroutine_AttemptPath(Vector3 start, Vector3 end)
    {
        Debug.Log("Starting path from   " + start.ToString() + " to " + end.ToString());
        ABPath path = ABPath.Construct(start, end, OnPathComplete);
        _AI.SetPath(path);
        yield return null;
    }

    private void OnPathComplete(Path path)
    {
        Debug.Log("Path calculated: " + path.vectorPath.Count + " nodes total");
    }
}
