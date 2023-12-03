using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System;
using static Constants;

public class EnvironmentManager: MonoBehaviour, IEncounterEventHandler
{
    //singleton
    private static EnvironmentManager _instance;
    public static EnvironmentManager Instance { get { return _instance; } }

    private List<IEncounterEventHandler> _eventHandlers;

    private List<EnvironmentInputHandler> _inputHandlers;

    private List<EnvironmentSpawnPoint> _spawnPoints;

    private List<EnvironmentObstacle> _obstacles;

    private EnvironmentInputData _inputData;

    private bool _allowUpdate = false;

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

        _eventHandlers = new List<IEncounterEventHandler>(GetComponentsInChildren<IEncounterEventHandler>());

        _eventHandlers.Remove(this);

        foreach (IEncounterEventHandler eventHandler in _eventHandlers)
        {
            Debug.Log("Found environment event handler: " + eventHandler.ToString());

            yield return eventHandler.Coroutine_PerformSetup();
        }

        _inputHandlers = new List<EnvironmentInputHandler>(GetComponentsInChildren<EnvironmentInputHandler>());

        //dispose of the setup navmesh after tiles are built
        GridGraph gridGraph = AstarPath.active.data.gridGraph;
        AstarPath.active.data.RemoveGraph(gridGraph);

        StartCoroutine(Coroutine_InputUpdate());

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
        CheckMouseClick();
    }

    private void CheckMouseClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (EncounterManager.Instance.GetCurrentState() == EncounterState.CHOOSE_ACTION)
            {
                if (_inputData.OnValidTile && _inputData.RangeType != MovementRangeType.None)
                {
                    ToggleInputHandlers(false);
                    EncounterManager.Instance.OnEnvironmentDestinationSelected(_inputData.TilePosition, _inputData.RangeType);
                }
                else
                {
                    Debug.Log("Click not handled");
                    Debug.Log(_inputData.ToString());
                }
            }
        }
    }

    private IEnumerator Coroutine_InputUpdate()
    {
        while(true)
        {
            if(_allowUpdate)
            {
                _inputData = EnvironmentInputData.Build();

                yield return CheckMousePosition();

                foreach (EnvironmentInputHandler inputHandler in _inputHandlers)
                {
                    yield return inputHandler.PerformInputUpdate(_inputData);
                }
            }

            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator CheckMousePosition()
    {
        //Bail if the mouse is blocked by UI
        if (EnvironmentUtil.CheckIsMouseBlocked()) { yield break; }

        //shoot a ray from the camera to the ground

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        foreach(RaycastHit hit in Physics.RaycastAll(ray, 100, LayerMask.GetMask("EnvironmentGround")))
        {
            Vector3 nodePosition;

            //find the closest node to the mouse location
            if (GetClosestNodeToPosition(hit.point, out nodePosition))
            {
                nodePosition.y = 0;

                _inputData.TilePosition = nodePosition;

                //if it's unoccupied, it's ok to highlight/path/click it
                if (IsPositionFree(nodePosition))
                {
                    _inputData.OnValidTile = true;

                    CharacterComponent currentCharacter = EncounterManager.Instance.GetCurrentCharacter();

                    if (currentCharacter != null)
                    {
                        //calculate the cost of the path if we made it to here
                        yield return Coroutine_CalculatePathCost(currentCharacter.GetWorldLocation(), nodePosition);

                        MovementRangeType rangeType;

                        if (EnvironmentUtil.IsWithinCharacterRange(_inputData.PathCost, currentCharacter, out rangeType))
                        {
                            _inputData.RangeType = rangeType;
                        }
                    }
                }
            }

            break;
        }

        yield return null;
    }

    private IEnumerator Coroutine_CalculatePathCost(Vector3 origin, Vector3 destination)
    {
        _inputData.PathCost = 0;
        _inputData.VectorPath = new List<Vector3>();

        ABPath path = ABPath.Construct(origin, destination);

        AstarPath.StartPath(path, true);

        yield return new WaitUntil(() => path.CompleteState == PathCompleteState.Complete);

        foreach (GraphNode pathNode in path.path)
        {
            Vector3 nodePosition = (Vector3) pathNode.position;

            if (!_inputData.VectorPath.Contains(nodePosition))
            {
                _inputData.VectorPath.Add(nodePosition);
                _inputData.PathCost += (int)path.GetTraversalCost(pathNode);
            }
        }

        _inputData.VectorPath.Add(destination);
    }

    //Encounter Events

    public IEnumerator Coroutine_EncounterStateUpdate(EncounterState stateID, EncounterModel model)
    {
        StopCoroutine(Coroutine_InputUpdate());

        switch (stateID)
        {
            case EncounterState.CHOOSE_ACTION:
                ToggleInputHandlers(true);
                break;
            case EncounterState.PERFORM_ACTION:
                ToggleInputHandlers(false);
                break;
            default:
                break;
        }

        foreach (IEncounterEventHandler eventHandler in _eventHandlers)
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

    private void ToggleInputHandlers(bool flag)
    {
        foreach(EnvironmentInputHandler inputHandler in _inputHandlers)
        {
            if(flag)
            {
                inputHandler.Activate();
            }
            else
            {
                inputHandler.Deactivate();
            }

        }

        _allowUpdate = flag;
    }
}
