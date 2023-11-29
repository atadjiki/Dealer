using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System;
using static Constants;

public struct EnvironmentInputData
{
    public bool IsValid;
    public Vector3 ClosestNode;
    public MovementRangeType RangeType;
    public int PathCost;

    public static EnvironmentInputData Build()
    {
        return new EnvironmentInputData()
        {
            IsValid = false,
            ClosestNode = Vector3.zero,
            RangeType = MovementRangeType.None,
            PathCost = -1,
        };
    }
}

public class EnvironmentManager: MonoBehaviour, IEncounterEventHandler
{
    //singleton
    private static EnvironmentManager _instance;
    public static EnvironmentManager Instance { get { return _instance; } }

    [SerializeField] private List<IEncounterEventHandler> EventHandlers;

    private List<EnvironmentSpawnPoint> _spawnPoints;

    private List<EnvironmentObstacle> _obstacles;

    //Setup
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    public IEnumerator Coroutine_Build()
    {
        yield return Coroutine_ScanNavmesh();
        yield return Coroutine_GatherEnvironmentObjects();

        EventHandlers = new List<IEncounterEventHandler>(GetComponentsInChildren<IEncounterEventHandler>());

        EventHandlers.Remove(this);

        foreach (IEncounterEventHandler eventHandler in EventHandlers)
        {
            Debug.Log("Found environment event handler: " + eventHandler.ToString());

            yield return eventHandler.Coroutine_PerformSetup();
        }

        //dispose of the setup navmesh after tiles are built
        GridGraph gridGraph = AstarPath.active.data.gridGraph;
        AstarPath.active.data.RemoveGraph(gridGraph);

        Debug.Log("Environment Ready");
    }

    private IEnumerator Coroutine_ScanNavmesh()
    {
        Debug.Log("Scanning NavMesh");
        GridGraph gridGraph = AstarPath.active.data.gridGraph;
        AstarPath.active.Scan(gridGraph);

        yield return new WaitWhile(() => AstarPath.active.isScanning);
    }

    private IEnumerator Coroutine_GatherEnvironmentObjects()
    {
        Debug.Log("Gathering Environment Objects...");

        _spawnPoints = new List<EnvironmentSpawnPoint>();
        _obstacles = new List<EnvironmentObstacle>();

        foreach(EnvironmentSpawnPoint spawnPoint in GetComponentsInChildren<EnvironmentSpawnPoint>())
        {
            Vector3 position = spawnPoint.transform.position;

            Vector3 result;
            if(GetClosestNodeToPosition(position, out result))
            {
                _spawnPoints.Add(spawnPoint);
            }
            else
            {
                Debug.Log("Failed to gather spawn point " + spawnPoint.name);
                Destroy(spawnPoint.gameObject);
            }
        }

        foreach (EnvironmentObstacle obstacle in GetComponentsInChildren<EnvironmentObstacle>())
        {
            Vector3 position = obstacle.transform.position;

            Vector3 result;
            if (GetClosestNodeToPosition(position, out result))
            {
                _obstacles.Add(obstacle);
            }
            else
            {
                Debug.Log("Failed to gather obstacle " + obstacle.name);
                Destroy(obstacle.gameObject);
            }
        }

        yield return null;
    }

    //Environment Input/Interactions

    private void Update()
    {
        if (EncounterManager.Instance.GetCurrentState() == EncounterState.CHOOSE_ACTION)
        {
            CheckMouseHighlight();
            CheckMouseClick();
        }
    }

    private void CheckMouseHighlight()
    {
        //Bail if the mouse is blocked by UI
        if (EnvironmentUtil.CheckIsMouseBlocked()) { return; }

        EnvironmentInputData InputData = EnvironmentInputData.Build();

        //shoot a ray from the camera to the ground
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100, LayerMask.GetMask("EnvironmentGround")))
        {
            Vector3 nodePosition;

            //find the closest node to the mouse location
            if (GetClosestNodeToPosition(hit.point, out nodePosition))
            {
                //if it's unoccupied, it's ok to highlight/path/click it
                if (IsPositionFree(nodePosition))
                {
                    InputData.IsValid = true;

                    CharacterComponent currentCharacter = EncounterManager.Instance.GetCurrentCharacter();

                    if(currentCharacter != null)
                    {
                        //calculate the path cost correctly here
                        int distance = (int)Vector3.Distance(nodePosition, currentCharacter.GetWorldLocation());

                        MovementRangeType rangeType;

                        if (EnvironmentUtil.IsWithinCharacterRange(distance, currentCharacter, out rangeType))
                        {
                            InputData.RangeType = rangeType;
                        }
                        else
                        {
                            //the tile is out of range for the character
                        }
                    }
                }
                else
                {
                    //the tile is occupied, so do nothing (or maybe highlight the next adjaecent node)
                }
            }
            else
            {
                //no node is available, so do nothing
            }
        }
    }

    private void CheckMouseClick()
    {
        if (EnvironmentUtil.CheckIsMouseBlocked()) { return; }

        if (!Input.GetMouseButtonDown(0)) return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100, LayerMask.GetMask("EnvironmentGround")))
        {
            if (!EnvironmentManager.Instance.IsPositionOccupied(hit.point))
            {
                StartCoroutine(Coroutine_OnDestinationSelected(hit.point));
            }
        }
    }

    private IEnumerator Coroutine_OnDestinationSelected(Vector3 destination)
    {
        CharacterComponent currentCharacter = EncounterManager.Instance.GetCurrentCharacter();

        int movementRange = currentCharacter.GetMovementRange();

        Vector3 origin = currentCharacter.GetWorldLocation();

        ABPath path = ABPath.Construct(origin, destination);

        AstarPath.StartPath(path, true);

        yield return new WaitUntil(() => path.CompleteState == PathCompleteState.Complete);

        int cost = 0;

        foreach (GraphNode pathNode in path.path)
        {
            cost += (int)path.GetTraversalCost(pathNode);
        }

        Debug.Log("Path cost is " + cost);

        MovementRangeType rangeType;
        if (EnvironmentUtil.IsWithinCharacterRange(cost, currentCharacter, out rangeType))
        {
            EncounterManager.Instance.OnEnvironmentDestinationSelected(destination, rangeType);
        }
        else
        {
            Debug.Log(destination.ToString() + " is out of range for " + currentCharacter.GetID());
        }
    }

    //Encounter Events

    public IEnumerator Coroutine_EncounterStateUpdate(EncounterState stateID, EncounterModel model)
    {
        foreach (IEncounterEventHandler eventHandler in EventHandlers)
        {
            yield return eventHandler.Coroutine_EncounterStateUpdate(stateID, model);
        }

        yield return null;
    }

    //Helpers/interface 
    public CharacterComponent SpawnCharacter(TeamID teamID, CharacterID characterID)
    {
        //find a spawn point to place the character
        //see if we have a marker available to spawn them in
        foreach (EnvironmentSpawnPoint spawnPoint in _spawnPoints)
        {
            Vector3 location;
            if (GetClosestNodeToPosition(spawnPoint.transform.position, out location))
            {
                if (spawnPoint.GetTeam() == teamID)
                {
                    GameObject characterObject = new GameObject(teamID + "_" + characterID);

                    CharacterComponent characterComponent = EnvironmentUtil.AddComponentByTeam(characterID, characterObject);

                    characterObject.transform.parent = this.transform;
                    characterObject.transform.localPosition = Vector3.zero;
                    characterObject.transform.localEulerAngles = spawnPoint.transform.eulerAngles;
                    characterObject.transform.position = location;

                    return characterComponent;
                }
            }
            else
            {
                Debug.Log("Couldnt find location to spawn character");
            }
        }

        return null;
    }

    public bool GetClosestNodeToPosition(Vector3 position, out Vector3 result)
    {
        GridGraph gridGraph = AstarPath.active.data.gridGraph;

        NNInfoInternal nnInfo = gridGraph.GetNearest(position);

        if (nnInfo.node != null && nnInfo.node != null)
        {
            result = (Vector3)nnInfo.node.position;
            return true;
        }
        else
        {
            result = Vector3.zero;
            return false;
        }
    }

    public bool IsPositionOccupied(Vector3 worldLocation)
    {
        Vector3 result;
        if(GetClosestNodeToPosition(worldLocation, out result ))
        {
            foreach (EnvironmentObstacle obstacle in _obstacles)
            {
                if (obstacle.ContainsPoint(result))
                {
                    return true;
                }
            }

            //will need to track characters as well TODO
        }

        return false;
    }

    public bool IsPositionFree(Vector3 worldLocation)
    {
        return !IsPositionOccupied(worldLocation);
    }

    public List<EnvironmentSpawnPoint> GetSpawnPoints()
    {
        return _spawnPoints;
    }

    public List<EnvironmentObstacle> GetObstacles()
    {
        return _obstacles;
    }
}
