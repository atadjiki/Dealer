using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using Pathfinding.RVO;
using UnityEditor;
using Constants;
using UnityEngine;

[RequireComponent(typeof(Seeker))]
[RequireComponent(typeof(AIPath))]

public class NavigatorComponent : MonoBehaviour
{
    //delegates
    public delegate void OnDestinationReached();
    public OnDestinationReached OnDestinationReachedDelegate;


    //pathfinding AI
    private Seeker _seeker;
    private AIPath _AI;

    private bool _initialized = false;

    public bool HasInitialized()
    {
        return _initialized;
    }

    public void Initialize(NPCComponent character)
    {
        character.OnNewDestinationDelegate += SetDestination;

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

        StartCoroutine(PerformMove());
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

    public bool IsMoving()
    {
        return !_AI.isStopped && !_AI.reachedEndOfPath;
    }

    public void HandleCharacterAction(Enumerations.CharacterAction action)
    {
        if(action == Enumerations.CharacterAction.None)
        {
            _AI.canMove = false;
        }
    }

    public IEnumerator PerformMove()
    {
        _AI.canMove = true;
        _AI.SearchPath();

        yield return new WaitForSeconds(0.2f);

        while (IsMoving() && GetDistanceToDestination() > 0.15f)
        {
            Debug.DrawLine(GetStartOfPath(), GetNextPointInPath(), Color.blue, Time.fixedDeltaTime);

            Debug.DrawLine(transform.position, GetDestination(), Color.green, Time.fixedDeltaTime);

            yield return new WaitForFixedUpdate();
        }

        _AI.canMove = false;

        OnDestinationReachedDelegate.Invoke();
    }
}
