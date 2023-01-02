using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using Pathfinding.RVO;
using UnityEditor;
using Constants;
using GameDelegates;
using UnityEngine;

[RequireComponent(typeof(Seeker))]
[RequireComponent(typeof(RichAI))]

public class NavigatorComponent : MonoBehaviour
{
    //delegates
    public DestinationReached OnDestinationReachedDelegate;

    private Enumerations.MovementState _movementState = Enumerations.MovementState.None;

    //pathfinding AI
    private Seeker _seeker;
    private RichAI _AI;

    //vars
    private float _remainingDistance;
    private float _checkTime;
    private float _maxCheckTime = 0.35f;

    private bool _initialized = false;

    public bool HasInitialized()
    {
        return _initialized;
    }

    public void Initialize(NPCComponent character)
    {
        character.OnNewDestination += SetDestination;

        StartCoroutine(PerformInitialize());
    }

    public IEnumerator PerformInitialize()
    {
        _seeker = GetComponent<Seeker>();
        _seeker.pathCallback += OnPathComplete;

        yield return new WaitUntil(() => _seeker != null);

        _AI = GetComponent<RichAI>();

        yield return new WaitUntil(() => _AI != null);

        AstarPath.active.ScanAsync();

        _initialized = true;
    }

    public void SetDestination(Vector3 destination)
    {
        _AI.destination = destination;

        GameObject navDecal = Instantiate<GameObject>(PrefabLibrary.GetCharacterNavDecal(), null);
        CharacterNavDecal decalComponent = navDecal.GetComponent<CharacterNavDecal>();
        decalComponent.Setup(this, destination);

        decalComponent.Show(Enumerations.Team.Player);

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

    public bool CanMove()
    {
        return _AI.canMove && _AI.hasPath;
    }

    public void HandleMovementState(Enumerations.MovementState state)
    {
        _movementState = state;

        if(state == Enumerations.MovementState.Moving)
        {
            _AI.canMove = true;
        }
        else
        {
            _AI.canMove = false;
        }
    }

    public IEnumerator PerformMove()
    {
        _AI.canMove = true;
        _AI.SearchPath();

        yield return new WaitForSeconds(0.2f);

        while (IsMoving() && GetDistanceToDestination() > _AI.radius && CanMove())
        {

            if (_checkTime > _maxCheckTime)
            {
               if(_remainingDistance == _AI.remainingDistance)
                {
                    Debug.Log("Stuck! Cancelling path");
                    break;
                }

                _checkTime = 0;
            }

            Debug.DrawLine(GetStartOfPath(), GetNextPointInPath(), Color.blue, Time.fixedDeltaTime);

            Debug.DrawLine(transform.position, GetDestination(), Color.green, Time.fixedDeltaTime);


            _remainingDistance = _AI.remainingDistance;
            _checkTime += Time.fixedDeltaTime;

            yield return new WaitForFixedUpdate();
        }

        _AI.canMove = false;

        OnDestinationReachedDelegate.Invoke();
    }

    private void OnPathComplete(Path path)
    {

    }

#if UNITY_EDITOR

    private void OnDrawGizmos()
    {
        if(_movementState != Enumerations.MovementState.None)
        {
            Handles.Label(this.transform.position, _movementState.ToString());
        }
    }
#endif
}
