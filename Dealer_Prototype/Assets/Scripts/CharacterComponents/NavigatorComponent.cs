using System;
using System.Collections;
using System.Collections.Generic;
using Constants;
using Pathfinding;
using UnityEngine;

//a class that deals with all things pathfinding related so that it's in one place
[RequireComponent(typeof(AIBase))]
[RequireComponent(typeof(Seeker))]
public class NavigatorComponent : MonoBehaviour
{
    internal AIBase _AI;
    internal Seeker _Seeker;
    private CharacterComponent parentCharacter;

    public enum MovementState { Stopped, Moving };
    public MovementState State = MovementState.Stopped;

    public delegate void OnReachedLocation(CharacterComponent character, MarkedLocation location);
    public OnReachedLocation onReachedLocation;

    private void Awake()
    {
        parentCharacter = GetComponentInParent<CharacterComponent>();
        _AI = GetComponentInChildren<AIPath>();
        _AI.gravity = Vector3.zero;
        _Seeker = GetComponentInChildren<Seeker>();

        _AI.autoRepath.mode = AutoRepathPolicy.Mode.EveryNSeconds;
        _AI.autoRepath.interval = 0.2f;

    }

    private Tuple<NNInfo, NNInfo> ValidateLocation(Vector3 location, bool checkPath, out bool success)
    {

        NNInfo NearestNode_origin = AstarPath.active.GetNearest(this.transform.position, NNConstraint.Default);
        NNInfo NearestNode_destination = AstarPath.active.GetNearest(location, NNConstraint.Default);

        //check distances
        //if (Vector3.Distance(NearestNode_origin.position, this.transform.position) > 0.1f)
        //{
        //    DebugManager.Instance.Print(DebugManager.Log.LogCharacter, "No nodes available around origin");

        //    success = false;
        //    return null;
        //}
        //else if (Vector3.Distance(NearestNode_destination.position, location) > 0.1f)
        //{
        //    DebugManager.Instance.Print(DebugManager.Log.LogCharacter, "No nodes available around destination");

        //    success = false;
        //    return null;
        //}

        if (checkPath)
        {
            success = PathUtilities.IsPathPossible(NearestNode_origin.node, NearestNode_destination.node);
            return null;
        }
        else
        {
            success = true;
            return new Tuple<NNInfo, NNInfo>(NearestNode_origin, NearestNode_destination);
        }
    }

    public bool TeleportToLocation(MarkedLocation markedLocation)
    {
        this.transform.position = markedLocation.transform.position;
        this.transform.rotation = markedLocation.transform.rotation;
        return true;
    }

    public bool MoveToLocation(MarkedLocation markedLocation, bool teleportOnFail)
    {
        bool success;
        Tuple<NNInfo, NNInfo> VectorPair = ValidateLocation(markedLocation.transform.position, false, out success);

        if (success)
        {
            State = MovementState.Moving;
            StartCoroutine(DoMoveToLocation(markedLocation, VectorPair.Item2.position));
            Debug.Log("movement success");
            return true;
        }
        else
        {
            DebugManager.Instance.Print(DebugManager.Log.LogCharacter, name + ": " + "Path not possible to " + markedLocation);

            if (teleportOnFail)
            {
                TeleportToLocation(markedLocation);
            }

            return false;
        }
    }

    private IEnumerator DoMoveToLocation(MarkedLocation markedLocation, Vector3 verifiedLocation)
    {
        _AI.destination = verifiedLocation;
        _AI.SearchPath(); // Start to search for a path to the destination immediately
        float timeStamp = Time.time;
        yield return new WaitForEndOfFrame();

        parentCharacter.GetAnimationComponent().FadeToAnimation(AnimationConstants.Anim.Walking, 0.15f, true);

        if (DebugManager.Instance.State_Navigator != DebugManager.State.None) DebugExtension.DebugWireSphere(verifiedLocation, Color.green, 1, 1, false);
        parentCharacter.OnNewDestination(verifiedLocation);

        // Wait until the agent has reached the destination
        while (true)
        {
            if (DebugManager.Instance.State_Navigator != DebugManager.State.None) DebugExtension.DebugWireSphere(verifiedLocation, Color.green, 0.25f, Time.fixedDeltaTime, false);

            yield return new WaitForEndOfFrame();

            float timeElapsed = Mathf.Abs(Time.time - timeStamp);

            if (timeElapsed > 0.15f && _AI.velocity == Vector3.zero)
            {
                break;
            }

            if (Vector3.Distance(this.transform.position, verifiedLocation) < 0.1f)
            {
                break;
            }
        }

        onReachedLocation(parentCharacter, markedLocation);

        // The agent has reached the destination now
        if (DebugManager.Instance.State_Navigator != DebugManager.State.None) DebugExtension.DebugWireSphere(verifiedLocation, Color.green, 1, 1, false);

        yield return new WaitForSeconds(0.1f);

        parentCharacter.GetAnimationComponent().FadeToAnimation(markedLocation.LocationAnim, 0f, false);

        yield return StartCoroutine(LerpToTransform(markedLocation.transform, 1.5f));

   
        State = MovementState.Stopped;
    }

    private IEnumerator LerpToTransform(Transform targetTransform, float lerpTime)
    {
        Transform initialTransform = this.transform;

        float time = 0;

        while (time < lerpTime)
        {
            transform.position = Vector3.Slerp(initialTransform.position, targetTransform.position, time/lerpTime);
            transform.rotation = Quaternion.Slerp(initialTransform.rotation, targetTransform.rotation, time/lerpTime);
            time += Time.deltaTime;
            yield return null;
        }

        this.transform.position = targetTransform.position;
        this.transform.rotation = targetTransform.rotation;

        yield return null;
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

