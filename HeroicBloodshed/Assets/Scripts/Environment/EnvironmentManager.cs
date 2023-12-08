using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System;
using static Constants;
using UnityEngine.EventSystems;

public class EnvironmentManager: MonoBehaviour, IEncounterEventHandler
{
    //singleton
    private static EnvironmentManager _instance;
    public static EnvironmentManager Instance { get { return _instance; } }

    private List<IEncounterEventHandler> _eventHandlers;

    private List<EnvironmentInputHandler> _inputHandlers;

    private Dictionary<Vector3, EnvironmentNode> _nodeMap;

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

        AstarPath.active.maxNearestNodeDistance = 12;

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

        _nodeMap = new Dictionary<Vector3, EnvironmentNode>();

        gridGraph.GetNodes((graphNode =>
        {
            if(graphNode.Walkable)
            {
                Vector3 position = (Vector3)graphNode.position;
                EnvironmentNode environmentNode = new EnvironmentNode();

                environmentNode.position = position;
                environmentNode.neighbors = EnvironmentUtil.GetNeighboringNodes(position);

                _nodeMap.Add((Vector3)graphNode.position, environmentNode);
            }
        }));
    }

    private IEnumerator Coroutine_GatherEnvironmentObjects()
    {
        Debug.Log("Gathering Environment Objects...");

        foreach(EnvironmentSpawnPoint spawnPoint in GetComponentsInChildren<EnvironmentSpawnPoint>())
        {
            Vector3 position = spawnPoint.transform.position;

            Vector3 result;
            if(EnvironmentUtil.GetClosestNodeInArea(position, new Vector3(2,2,2), out result))
            {
                Debug.Log("Spawn Point gathered at " + result.ToString());

                if (_nodeMap.ContainsKey(result))
                {
                    _nodeMap[result].spawnPoint = spawnPoint;
                }
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
            if (EnvironmentUtil.GetClosestNodeToPosition(position, out result))
            {
                obstacle.Setup();

                if(_nodeMap.ContainsKey(result))
                {
                    Debug.Log("Obstacle gathered at " + result.ToString());
                    _nodeMap[result].obstacle = obstacle;
                }

                foreach(Vector3 node in EnvironmentUtil.GetNodesInBounds(obstacle.GetBounds()))
                {
                    obstacle.AddCoverDecal(node);
                }
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

    private IEnumerator Coroutine_InputUpdate()
    {
        while(true)
        {
            if(_allowUpdate)
            {
                _inputData = EnvironmentInputData.Build();

                yield return CheckMousePosition();

                yield return CheckMouseClick();

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
            if (EnvironmentUtil.GetClosestNodeToPosition(hit.point, out nodePosition))
            {
                nodePosition.y = 0;

                _inputData.NodePosition = nodePosition;

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

    private IEnumerator CheckMouseClick()
    {
        if (Input.GetMouseButton(0))
        {
            Debug.Log("click");

            if (EncounterManager.Instance.ShouldAllowInput())
            {
                if (_inputData.OnValidTile && _inputData.RangeType != MovementRangeType.None)
                {
                    ToggleInputHandlers(false);
                    EncounterManager.Instance.OnEnvironmentDestinationSelected(_inputData.NodePosition, _inputData.RangeType);
                }
                else
                {
                    Debug.Log("Click not handled");
                    Debug.Log(_inputData.ToString());
                }
            }
        }

        yield return null;
    }

    private IEnumerator Coroutine_CalculatePathCost(Vector3 origin, Vector3 destination)
    {
        _inputData.PathCost = 0;
        _inputData.PathToHighlightedNode = new List<Vector3>();

        ABPath path = ABPath.Construct(origin, destination);

        AstarPath.StartPath(path, true);

        yield return new WaitUntil(() => path.CompleteState == PathCompleteState.Complete);

        foreach (GraphNode pathNode in path.path)
        {
            Vector3 nodePosition = (Vector3) pathNode.position;

            if (!_inputData.PathToHighlightedNode.Contains(nodePosition))
            {
                _inputData.PathToHighlightedNode.Add(nodePosition);
                _inputData.PathCost += (int)path.GetTraversalCost(pathNode);
            }
        }

        _inputData.PathToHighlightedNode.Add(destination);
    }

    private IEnumerator Coroutine_CalculateRadiusBounds()
    {
        _inputData.RadiusMap = new Dictionary<Vector3, int>();

        GridGraph gridGraph = AstarPath.active.data.gridGraph;

        CharacterComponent currentCharacter = EncounterManager.Instance.GetCurrentCharacter();

        Vector3 origin = currentCharacter.GetWorldLocation();

        Bounds searchBounds = new Bounds(origin, new Vector3(24, 1, 24));

        foreach (EnvironmentObstacle obstacle in GetObstacles())
        {
            obstacle.ToggleDecal(false);
        }

        //find the distance between the character and every walkable node in the grid graph (yikes)
        foreach (GraphNode graphNode in gridGraph.GetNodesInRegion(searchBounds))
        {
            StartCoroutine(Coroutine_PerformPathSearch(origin, graphNode));
        }

        yield return null;
    }

    private IEnumerator Coroutine_PerformPathSearch(Vector3 origin, GraphNode graphNode)
    {
        CharacterComponent currentCharacter = EncounterManager.Instance.GetCurrentCharacter();

        //dont count nodes that are invalid anyway
        if (graphNode.Walkable)
        {
            Vector3 nodePosition = (Vector3)graphNode.position;

            //check if there is an obstacle or character blocking this node
            if (IsPositionFree(nodePosition))
            {
                ABPath path = ABPath.Construct(origin, nodePosition);

                path.heuristic = Heuristic.Euclidean;
                path.nnConstraint = EnvironmentUtil.BuildConstraint();

                AstarPath.StartPath(path, true);

                yield return new WaitUntil(() => path.CompleteState == PathCompleteState.Complete);

                int cost = 0;

                foreach (GraphNode pathNode in path.path)
                {
                    cost += (int)path.GetTraversalCost(pathNode);
                }

                //if the path to this node is available, add it to our list 
                MovementRangeType rangeType;
                if (EnvironmentUtil.IsWithinCharacterRange(cost, currentCharacter, out rangeType))
                {
                    if (!_inputData.RadiusMap.ContainsKey(nodePosition))
                    {
                        _inputData.RadiusMap.Add(nodePosition, cost);

                        foreach (EnvironmentObstacle obstacle in GetObstacles())
                        {
                            if(obstacle.IsNeighborOf(nodePosition))
                            {
                                obstacle.ToggleDecal(true);
                            }
                        }
                    }
                }
            }
        }

        yield return null;
    }

    //Encounter Events

    public IEnumerator Coroutine_EncounterStateUpdate(EncounterState stateID, EncounterModel model)
    {
        StopCoroutine(Coroutine_InputUpdate());

        switch (stateID)
        {
            case EncounterState.CHOOSE_ACTION:
                if(EncounterManager.Instance.ShouldAllowInput())
                {
                    yield return Coroutine_CalculateRadiusBounds();
                    ToggleInputHandlers(true);
                }
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
    public bool FindSpawnLocationForCharacter(CharacterComponent character, out Vector3 location, out Quaternion rotation)
    {
        //find a spawn point to place the character
        //see if we have a marker available to spawn them in
        foreach (EnvironmentSpawnPoint spawnPoint in GetSpawnPoints())
        {
            if (EnvironmentUtil.GetClosestNodeInArea(spawnPoint.transform.position, new Vector3(1f,1f, 1f), out location))
            {
                if (spawnPoint.GetTeam() == character.GetTeam())
                {
                    rotation = spawnPoint.transform.rotation;
                    return true;
                }
            }
            else
            {
                Debug.Log("Couldnt find location to placecharacter");
            }
        }

        location = Vector3.zero;
        rotation = Quaternion.identity;
        return false;
    }

    public bool IsPositionOccupied(Vector3 worldLocation)
    {
        Vector3 result;
        if(EnvironmentUtil.GetClosestNodeToPosition(worldLocation, out result ))
        {
            foreach (EnvironmentObstacle obstacle in GetObstacles())
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
        List<EnvironmentSpawnPoint> spawnPoints = new List<EnvironmentSpawnPoint>();

        foreach(EnvironmentNode environmentNode in _nodeMap.Values)
        {
            if(environmentNode.spawnPoint != null)
            {
                spawnPoints.Add(environmentNode.spawnPoint);
            }
        }

        return spawnPoints;
    }

    public List<EnvironmentObstacle> GetObstacles()
    {
        List<EnvironmentObstacle> obstacles = new List<EnvironmentObstacle>();

        foreach (EnvironmentNode environmentNode in _nodeMap.Values)
        {
            if (environmentNode.obstacle != null)
            {
                obstacles.Add(environmentNode.obstacle);
            }
        }

        return obstacles;
    }

    public EnvironmentObstacle GetObstacleAt(Vector3 position)
    {
        if(_nodeMap.ContainsKey(position))
        {
            EnvironmentNode environmentNode = _nodeMap[position];

            return environmentNode.obstacle;
        }

        return null;
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

    public EnvironmentInputData GetInputData()
    {
        return _inputData;
    }
}
