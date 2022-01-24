using System;
using System.Collections;
using System.Collections.Generic;
using Constants;
using Pathfinding;
using UnityEngine;

//a class that deals with all things pathfinding related so that it's in one place
[RequireComponent(typeof(AIBase))]
[RequireComponent(typeof(Seeker))]
[RequireComponent(typeof(LineRenderer))]
public class NavigatorComponent : MonoBehaviour
{
    internal AIBase _AI;
    internal Seeker _Seeker;
    private CharacterComponent parentCharacter;
    private LineRenderer pathRenderer;

    private HashSet<GameObject> NavPointPrefabs;

    public enum MovementState { Stopped, Moving };
    public MovementState State = MovementState.Stopped;

    private void Awake()
    {
        parentCharacter = GetComponentInParent<CharacterComponent>();
        _AI = GetComponentInChildren<AIPath>();
        _AI.gravity = Vector3.zero;
        _Seeker = GetComponentInChildren<Seeker>();

        NavPointPrefabs = new HashSet<GameObject>();

        pathRenderer = GetComponent<LineRenderer>();
        pathRenderer.positionCount = 2;

        _AI.autoRepath.mode = AutoRepathPolicy.Mode.EveryNSeconds;
        _AI.autoRepath.interval = 0.2f;

    }

    private void FixedUpdate()
    {
        if (DebugManager.Instance.State_Navigator != DebugManager.State.None && PlayableCharacterManager.Instance.GetSelectedCharacter() == parentCharacter && pathRenderer != null && _Seeker != null && parentCharacter != null)
        {
            if (State == MovementState.Moving)
            {
                Path path = _Seeker.GetCurrentPath();

                if (path != null)
                {
                    List<Vector3> vectorPath = path.vectorPath;

                    if (vectorPath.Count > 2)
                    {
                        Vector3[] simplifiedPath = new Vector3[3];

                        //initial position
                        simplifiedPath[0] = _AI.position;
                        simplifiedPath[1] = vectorPath[vectorPath.Count / 2];
                        simplifiedPath[2] = _AI.destination;

                        pathRenderer.positionCount = simplifiedPath.Length;
                        pathRenderer.SetPositions(simplifiedPath);
                    }
                }
            }
            else
            {
                pathRenderer.positionCount = 0;
            }
        }
    }

    public bool MoveToRandomLocation()
    {
        return MoveToLocation(PickRandomPoint());
    }

    private Tuple<NNInfo, NNInfo> ValidateLocation(Vector3 location, bool checkPath, out bool success)
    {

        if (Vector3.Distance(this.transform.position, location) > parentCharacter.moveRadius)
        {
            success = false;
            return null;
        }

        NNInfo NearestNode_origin = AstarPath.active.GetNearest(this.transform.position, NNConstraint.Default);
        NNInfo NearestNode_destination = AstarPath.active.GetNearest(location, NNConstraint.Default);

        //check distances
        if (Vector3.Distance(NearestNode_origin.position, this.transform.position) > 0.1f)
        {
            DebugManager.Instance.Print(DebugManager.Log.LogCharacter, "No nodes available around origin");

            success = false;
            return null;
        }
        else if (Vector3.Distance(NearestNode_destination.position, location) > 0.1f)
        {
            DebugManager.Instance.Print(DebugManager.Log.LogCharacter, "No nodes available around destination");

            success = false;
            return null;
        }

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

    public bool TeleportToLocation(Transform transform)
    {
        bool success;
        Tuple<NNInfo, NNInfo> VectorPair = ValidateLocation(transform.position, false, out success);

        if (success)
        {
            this.transform.position = VectorPair.Item2.position;
         //   this.transform.rotation = transform.rotation;
            return true;
        }
        else
        {
            DebugManager.Instance.Print(DebugManager.Log.LogCharacter, name + ": " + "Path not possible to " + transform.position);
            return false;
        }
    }

    public bool MoveToLocation(Vector3 location)
    {
        bool success;
        Tuple<NNInfo, NNInfo> VectorPair = ValidateLocation(location, false, out success);

        if (success)
        {
            State = MovementState.Moving;
            StartCoroutine(DoMoveToLocation(VectorPair.Item2.position));
            return true;
        }
        else
        {
            DebugManager.Instance.Print(DebugManager.Log.LogCharacter, name + ": " + "Path not possible to " + location);
            return false;
        }
    }

    private IEnumerator DoMoveToLocation(Vector3 Destination)
    {
        _AI.destination = Destination;
        _AI.SearchPath(); // Start to search for a path to the destination immediately
        float timeStamp = Time.time;
        yield return new WaitForEndOfFrame();

        //get rid of any existing prefabs that are out there first
        foreach (GameObject todestroy in NavPointPrefabs)
        {
            Destroy(todestroy);
        }

        if (PlayableCharacterManager.Instance.GetSelectedCharacter() == parentCharacter)
            SpawnNavPointPrefab(Destination);

        parentCharacter.FadeToAnimation(AnimationConstants.Anim.Walking, 0.15f, true);

        if (DebugManager.Instance.State_Navigator != DebugManager.State.None) DebugExtension.DebugWireSphere(Destination, Color.green, 1, 1, false);
        parentCharacter.OnNewDestination(Destination);

        // Wait until the agent has reached the destination
        while (true)
        {
            if (DebugManager.Instance.State_Navigator != DebugManager.State.None) DebugExtension.DebugWireSphere(Destination, Color.green, 0.25f, Time.fixedDeltaTime, false);

            yield return new WaitForEndOfFrame();

            float timeElapsed = Mathf.Abs(Time.time - timeStamp);

            if (timeElapsed > 0.15f && _AI.velocity == Vector3.zero)
            {
                break;
            }

            if (Vector3.Distance(this.transform.position, Destination) < 0.1f)
            {
                break;
            }
        }

        // The agent has reached the destination now
        if (DebugManager.Instance.State_Navigator != DebugManager.State.None) DebugExtension.DebugWireSphere(Destination, Color.green, 1, 1, false);

        parentCharacter.FadeToAnimation(AnimationConstants.Anim.Idle, 0.15f, false);

        yield return new WaitForSeconds(0.1f);

        parentCharacter.OnDestinationReached(Destination);
        State = MovementState.Stopped;

        //get rid of any existing prefabs that are out there first
        foreach (GameObject todestroy in NavPointPrefabs)
        {
            Destroy(todestroy);
        }

        pathRenderer.positionCount = 0;

    }

    private Vector3 PickRandomPoint()
    {
        var point = UnityEngine.Random.onUnitSphere * UnityEngine.Random.Range(parentCharacter.moveRadius, parentCharacter.moveRadius * 1.5f);
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

    private void SpawnNavPointPrefab(Vector3 prefabLocation)
    {
        GameObject NavPointEffect = PrefabFactory.CreatePrefab(RegistryID.NavPoint, prefabLocation, Quaternion.identity, null);
        NavPointPrefabs.Add(NavPointEffect);
    }
}
