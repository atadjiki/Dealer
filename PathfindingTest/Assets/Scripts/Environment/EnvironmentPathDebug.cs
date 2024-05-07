using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using static Constants;

public class EnvironmentPathDebug : MonoBehaviour
{
    private Seeker _seeker;
    private AILerp _AI;

    private TileGraph _graph;

    private void Start()
    {
        _seeker = GetComponentInChildren<Seeker>();

        _AI = GetComponentInChildren<AILerp>();
        _AI.canSearch = false;

        _graph = (TileGraph)AstarPath.active.graphs[0];

        StartCoroutine(Coroutine_AttemptRandomPath());
    }

    private IEnumerator Coroutine_AttemptRandomPath()
    {
        while(true)
        {
            GraphNode end = _graph.GetRandomNode();

            Debug.Log("Starting path from   " + _AI.position + " to " + end.position);

            ABPath path = ABPath.Construct(_AI.position, ((Vector3)end.position), OnPathComplete);
            _AI.SetPath(path);

            yield return new WaitForSeconds(0.2f);

            _AI.canMove = true;

            yield return new WaitUntil(() => _AI.reachedEndOfPath);

            Debug.Log("Reached destination");

            _AI.canMove = false;

            yield return new WaitForSeconds(1.0f);
        }
    }

    private void OnPathComplete(Path path)
    {
        //Debug.Log("Path calculated: " + path.vectorPath.Count + " nodes total");
    }
}
