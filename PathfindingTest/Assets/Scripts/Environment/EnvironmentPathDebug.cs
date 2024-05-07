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
        _seeker = GetComponent<Seeker>();
        _AI = GetComponent<AIPath>();

        _graph = (TileGraph) AstarPath.active.graphs[0];

        StartCoroutine(Coroutine_AttemptPath(Start.position, End.position));
    }

    private IEnumerator Coroutine_AttemptPath(Vector3 start, Vector3 end)
    {
        ABPath path = new ABPath();
        path.startPoint = start;
        path.endPoint = end;

        Debug.Log("Starting path from   " + start.ToString() + " to " + end.ToString());
        _seeker.StartPath(path, OnPathComplete);

        yield return null;
    }

    private void OnPathComplete(Path path)
    {
        Debug.Log("Path Complete!   " + path.ToString());
        _AI.SetPath(path);
    }
}
