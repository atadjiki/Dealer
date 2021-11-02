using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;


[RequireComponent(typeof(Seeker))]
[RequireComponent(typeof(AIPath))]
[RequireComponent(typeof(Animator))]
public class CharacterComponent : MonoBehaviour
{
    public enum State { Idle, Moving, Interacting, Talking, Unavailable, Sitting };
    public State CurrentState;

    internal AIBase _AI;
    internal Seeker _Seeker;
    private Animator _animator;

    internal float moveRadius = 30;

    public AnimationConstants.Animations DefaultAnimation = AnimationConstants.Animations.Idle;

    internal void Initialize()
    {
        _AI = GetComponent<AIPath>();
        _Seeker = GetComponent<Seeker>();
        _animator = GetComponent<Animator>();

        PlayDefaultAnimation();
        CurrentState = State.Idle;

        _AI.gravity = Vector3.zero;
    }

    public void PlayDefaultAnimation()
    {
        FadeToAnimation(AnimationConstants.GetAnimByEnum(DefaultAnimation), 0.3f);
    }

    internal bool CanCharacterUpdate()
    {

        return true;

    }

    public bool MoveToLocation(Vector3 location)
    {
        if (Vector3.Distance(this.transform.position, location) > moveRadius) return false;

        NNInfo NearestNode_origin = AstarPath.active.GetNearest(this.transform.position, NNConstraint.Default);
        NNInfo NearestNode_destination = AstarPath.active.GetNearest(location, NNConstraint.Default);

        //check distances
        if (Vector3.Distance(NearestNode_origin.position, this.transform.position) > 1)
        {
            if(DebugManager.Instance.LogCharacter) Debug.Log("No nodes available around origin");
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

    public virtual void OnNewDestination(Vector3 destination) { }
    public virtual void OnDestinationReached(Vector3 destination) { }

    public IEnumerator DoMoveToLocation(Vector3 Destination)
    {
        ToMoving();
        _AI.destination = Destination;
        _AI.SearchPath(); // Start to search for a path to the destination immediately

        OnNewDestination(Destination);

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
        OnDestinationReached(Destination);

        ToIdle();
    }

    public void ToIdle()
    {
        CurrentState = State.Idle;
        _animator.CrossFade(AnimationConstants.Idle, 0.5f);
        _AI.canMove = false;
    }

    public void ToMoving()
    {
        CurrentState = State.Moving;
        _animator.CrossFade(AnimationConstants.Walking, 0.1f);
        _AI.canMove = true;
    }

    public void ToInteracting()
    {
        StopAllCoroutines();
        CurrentState = State.Interacting;
        _animator.CrossFade(AnimationConstants.ButtonPush, 0.3f);
        _AI.canMove = false;

    }

    public void ToTalking()
    {
        StopAllCoroutines();
        CurrentState = State.Talking;
        _animator.CrossFade(AnimationConstants.Talking, 0.3f);
        _AI.canMove = false;
    }

    public void ToSitting()
    {
        StopAllCoroutines();
        CurrentState = State.Sitting;
        FadeToAnimation(AnimationConstants.Male_Sitting_2, 0.35f);
        _AI.canMove = false;
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

    public void FadeToAnimation(string animation, float time)
    {
        _animator.CrossFade(animation, time);
    }
}
