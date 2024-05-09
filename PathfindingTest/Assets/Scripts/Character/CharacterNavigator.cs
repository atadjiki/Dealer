using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

[RequireComponent(typeof(AILerp))]
public class CharacterNavigator : MonoBehaviour
{
    public delegate void OnDestinationReached(CharacterNavigator navigator);
    public OnDestinationReached DestinationReachedCallback;

    private Seeker _seeker;
    private AILerp _AI;

    private void Awake()
    {
        _seeker = GetComponentInChildren<Seeker>();

        _AI = GetComponentInChildren<AILerp>();
        _AI.canSearch = false;
        _AI.canMove = false;
    }

    public void MoveTo(Vector3 destination)
    {
        StopAllCoroutines();

        StartCoroutine(Coroutine_MoveTo(destination));
    }

    public void Teleport(Vector3 destination)
    {
        _AI.Teleport(destination);
    }

    public void SetSpeed(float speed)
    {
        _AI.speed = speed;
    }

    private IEnumerator Coroutine_MoveTo(Vector3 destination)
    {
        Vector3 origin = _AI.position;

        ABPath path = ABPath.Construct(origin, destination, OnPathComplete);
        _AI.SetPath(path);

        Debug.Log("Starting path from   " + _AI.position + " to " + destination);
        yield return new WaitForFixedUpdate();

        _AI.canMove = true;
        yield return new WaitUntil(() => _AI.reachedEndOfPath);
        _AI.canMove = false;

        Debug.Log("Reached destination");

        if(DestinationReachedCallback != null)
        {
            DestinationReachedCallback.Invoke(this);
        }
    }

    private void OnPathComplete(Path path)
    {
        Debug.Log("Path calculated: " + path.vectorPath.Count + " nodes total");
    }
}
