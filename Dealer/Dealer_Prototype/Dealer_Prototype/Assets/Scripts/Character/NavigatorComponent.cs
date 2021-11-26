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

    private void Awake()
    {
        parentCharacter = GetComponentInParent<CharacterComponent>();
        _AI = GetComponentInChildren<AIPath>();
        _AI.gravity = Vector3.zero;
        _Seeker = GetComponentInChildren<Seeker>();

        NavPointPrefabs = new HashSet<GameObject>();

        pathRenderer = GetComponent<LineRenderer>();
        pathRenderer.positionCount = 2;
        
    }

    private void Update()
    {
        if (NPCManager.Instance.GetSelectedNPC() == parentCharacter && pathRenderer != null && _Seeker != null && parentCharacter != null)
        {
            if (parentCharacter.GetCurrentState() == Constants.CharacterConstants.State.Moving)
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

        //get rid of any existing prefabs that are out there first
        foreach (GameObject todestroy in NavPointPrefabs)
        {
            Destroy(todestroy);
        }

        if (NPCManager.Instance.GetSelectedNPC() == parentCharacter)
            SpawnNavPointPrefab(Destination);

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

        //get rid of any existing prefabs that are out there first
        foreach (GameObject todestroy in NavPointPrefabs)
        {
            Destroy(todestroy);
        }

        pathRenderer.positionCount = 0;

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

    private void SpawnNavPointPrefab(Vector3 prefabLocation)
    {
        GameObject NavPointEffect = PrefabFactory.Instance.CreatePrefab(RegistryID.NavPoint, prefabLocation, Quaternion.identity, null);
        NavPointPrefabs.Add(NavPointEffect);
    }
}
