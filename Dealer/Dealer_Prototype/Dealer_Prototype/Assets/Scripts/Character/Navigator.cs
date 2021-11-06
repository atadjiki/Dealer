using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

//a class that deals with all things pathfinding related so that it's in one place
[RequireComponent(typeof(AIBase))]
[RequireComponent(typeof(Seeker))]
public class Navigator : MonoBehaviour
{
    internal AIBase _AI;
    internal Seeker _Seeker;
    private CharacterComponent parentCharacter;

    private void Awake()
    {
        parentCharacter = GetComponentInParent<CharacterComponent>();
        _AI = GetComponentInChildren<AIPath>();
        _Seeker = GetComponentInChildren<Seeker>();

        _AI.gravity = Vector3.zero;
    }

    private Vector3 PickRandomPoint()
    {
        var point = Random.onUnitSphere * Random.Range(parentCharacter.moveRadius, parentCharacter.moveRadius * 1.5f);
        point.y = 0;
        point += this.transform.position;

        var graph = AstarPath.active.data.recastGraph;

        if (graph != null)
        {
            return graph.GetNearest(point, NNConstraint.Default).clampedPosition;
        }
        else
        {
            return point;
        }
    }

    public bool MoveToRandomLocation()
    {
        return MoveToLocation(PickRandomPoint());
    }

    public bool MoveToLocation(Vector3 location)
    {
        if (Vector3.Distance(this.transform.position, location) > parentCharacter.moveRadius) return false;

        NNInfo NearestNode_origin = AstarPath.active.GetNearest(this.transform.position, NNConstraint.Default);
        NNInfo NearestNode_destination = AstarPath.active.GetNearest(location, NNConstraint.Default);

        //check distances
        if (Vector3.Distance(NearestNode_origin.position, this.transform.position) > 1)
        {
            if (DebugManager.Instance.LogCharacter) Debug.Log("No nodes available around origin");
            return false;
        }
        else if (Vector3.Distance(NearestNode_destination.position, location) > 1)
        {
            if (DebugManager.Instance.LogCharacter) Debug.Log("No nodes available around destination");
            return false;
        }

        if (PathUtilities.IsPathPossible(NearestNode_origin.node, NearestNode_destination.node))
        {
            StartCoroutine(DoMoveToLocation(location));
            return true;
        }
        else
        {
            if (DebugManager.Instance.LogCharacter) Debug.Log(name + ": " + "Path not possible between " + NearestNode_origin.position + " and " + NearestNode_destination.position);
            return false;
        }
    }

    public IEnumerator DoMoveToLocation(Vector3 Destination)
    {
        parentCharacter.ToMoving();
        _AI.destination = Destination;
        _AI.SearchPath(); // Start to search for a path to the destination immediately

        parentCharacter.OnNewDestination(Destination);

        // Wait until the agent has reached the destination
        while (true)
        {
            yield return null;

            if (Vector3.Distance(this.transform.position, _AI.destination) < 1)
            {
                break;
            }
        }

        // The agent has reached the destination now
        parentCharacter.OnDestinationReached(Destination);

        parentCharacter.ToIdle();
    }

    public void ToggleMovement(bool flag)
    {
        if (flag)
        {
            _AI.isStopped = false;
        }
        else
        {
            _AI.isStopped = true;
        }
    }

    public void SetCanMove(bool flag)
    {
        _AI.canMove = flag;
    }
}
