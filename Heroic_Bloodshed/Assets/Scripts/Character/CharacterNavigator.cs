using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using static Constants;

[RequireComponent(typeof(AILerp))]
public class CharacterNavigator : MonoBehaviour
{
    public delegate void OnDestinationReached(CharacterNavigator navigator);
    public OnDestinationReached DestinationReachedCallback;

    public delegate void OnBeginMovementPath(MovementPathInfo info);
    public OnBeginMovementPath OnBeginMovementPathCallback;

    private Seeker _seeker;
    private AILerp _AI;
    private bool _pathCalculated;

    private void Awake()
    {
        _seeker = GetComponentInChildren<Seeker>();

        //_seeker.traversableTags = GetTraversibleTagMask();

        _AI = GetComponentInChildren<AILerp>();
        _AI.canSearch = false;
        _AI.canMove = false;
    }

    public void Teleport(Vector3 destination)
    {
        _AI.Teleport(destination);
    }

    public Vector3 GetWorldLocation()
    {
        return _AI.position;
    }

    public void SetSpeed(float speed)
    {
        _AI.speed = speed;
    }

    public IEnumerator Coroutine_MoveTo(Vector3 destination)
    {
        _pathCalculated = false;

        Vector3 origin = _AI.position;
        ABPath totalPath = ABPath.Construct(origin, destination, OnPathComplete);
        _AI.SetPath(totalPath);

        Debug.Log("Calculated total path");
        yield return new WaitUntil(() => _pathCalculated);
        _pathCalculated = false;


        foreach (MovementPathInfo info in EnvironmentUtil.CreatePathQueue(totalPath))
        {
            if(OnBeginMovementPathCallback != null)
            {
                OnBeginMovementPathCallback.Invoke(info);
            }

            ABPath subPath = ABPath.Construct(info.GetStart(), info.GetEnd(), OnPathComplete);
            _AI.SetPath(subPath);

            yield return new WaitUntil(() => _pathCalculated);

            Debug.Log("Starting " + info.PathType.ToString()  + " path from  " + _AI.position + " to " + destination);
            yield return new WaitForFixedUpdate();

            _AI.canMove = true;
            yield return new WaitUntil(() => _AI.reachedEndOfPath);
            _AI.canMove = false;

            Debug.Log("completed subpath " + info.PathType.ToString());

          //  Teleport(destination);

            _pathCalculated = false;
        }

        if (DestinationReachedCallback != null)
        {
            DestinationReachedCallback.Invoke(this);
        }
    }

    private void OnPathComplete(Path path)
    {
        Debug.Log("Path calculated: " + path.vectorPath.Count + " nodes total");
        _pathCalculated = true;
    }
}
