using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

using static Constants;
using System;
using Random = UnityEngine.Random;

public class EnvironmentManager: MonoBehaviour, IEncounterEventHandler
{
    [Header("Prefabs")]
    public List<GameObject> Prefabs_InputHandlers;

    //singleton
    private static EnvironmentManager _instance;
    public static EnvironmentManager Instance { get { return _instance; } }

    private List<IEncounterEventHandler> _eventHandlers;

    private List<EnvironmentInputHandler> _inputHandlers;

    private EnvironmentInputData _inputData;

    public Dictionary<Vector3, List<EnvironmentCoverData>> _coverMap;

    private bool _allowUpdate = false;

    public static bool IsActive()
    {
        return Instance != null;
    }

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

        yield return Coroutine_UpdateCoverMap();

        _eventHandlers = new List<IEncounterEventHandler>(GetComponentsInChildren<IEncounterEventHandler>());

        _eventHandlers.Remove(this);

        foreach (IEncounterEventHandler eventHandler in _eventHandlers)
        {
            Debug.Log("Found environment event handler: " + eventHandler.ToString());

            yield return eventHandler.Coroutine_PerformSetup();
        }

        _inputHandlers = new List<EnvironmentInputHandler>();

        foreach (GameObject prefab in Prefabs_InputHandlers)
        {
            GameObject childObject = Instantiate<GameObject>(prefab, this.transform);

            yield return new WaitUntil(() => childObject.GetComponent<EnvironmentInputHandler>());

            EnvironmentInputHandler inputHandler = childObject.GetComponent<EnvironmentInputHandler>();

            _inputHandlers.Add(inputHandler);
        }

        AstarPath.active.maxNearestNodeDistance = 12;

        StartCoroutine(Coroutine_InputUpdate());

       // _allowUpdate = true;

        Debug.Log("Environment Ready");
    }

    private IEnumerator Coroutine_ScanNavmesh()
    {
        EnvironmentUtil.ProcessEnvironment();

        yield return new WaitWhile(() => AstarPath.active.isScanning);
    }

    private IEnumerator Coroutine_UpdateCoverMap()
    {
        _coverMap = new Dictionary<Vector3, List<EnvironmentCoverData>>();

        GridGraph gridGraph = AstarPath.active.data.gridGraph;

        //check each walkable node for its cover values
        foreach (GraphNode node in gridGraph.nodes)
        {
            if (node.Walkable)
            {
                Vector3 origin = (Vector3)node.position;
                List<EnvironmentCoverData> coverDirections = EnvironmentUtil.GetNodeCoverData(node);

                if (coverDirections.Count > 0)
                {
                    _coverMap.Add(origin, coverDirections);
                }
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
            GraphNode graphNode;

            //find the closest node to the mouse location
            if (EnvironmentUtil.GetClosestGraphNodeToPosition(hit.point, out graphNode))
            {
                _inputData.NodePosition = (Vector3) graphNode.position;

                EnvironmentUtil.GetNodeCoverData(graphNode);

                //if it's unoccupied, it's ok to highlight/path/click it
                if (EnvironmentUtil.IsPositionFree(_inputData.NodePosition))
                {
                    _inputData.OnValidTile = true;

                    if(EncounterManager.Instance != null && EncounterManager.Instance.GetCurrentCharacter() != null)
                    {
                        CharacterComponent currentCharacter = EncounterManager.Instance.GetCurrentCharacter();

                        if (currentCharacter != null)
                        {
                            //calculate the cost of the path if we made it to here
                            yield return Coroutine_CalculatePathCost(currentCharacter.GetWorldLocation(), _inputData.NodePosition);

                            MovementRangeType rangeType;

                            if (EnvironmentUtil.IsWithinCharacterRange(_inputData.PathCost, currentCharacter, out rangeType))
                            {
                                _inputData.RangeType = rangeType;
                            }
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

        path.heuristic = Heuristic.Euclidean;
        path.nnConstraint = EnvironmentUtil.BuildConstraint();

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
    }

    private IEnumerator Coroutine_CalculateRadiusBounds()
    {
        _inputData.RadiusMaps = new Dictionary<MovementRangeType, Dictionary<Vector3, int>>();

        foreach (MovementRangeType rangeType in Enum.GetValues(typeof(MovementRangeType)))
        {
            if(rangeType != MovementRangeType.None)
            {
                _inputData.RadiusMaps.Add(rangeType, new Dictionary<Vector3, int>());
            }
        }

        GridGraph gridGraph = AstarPath.active.data.gridGraph;

        CharacterComponent currentCharacter = EncounterManager.Instance.GetCurrentCharacter();

        Vector3 origin = currentCharacter.GetWorldLocation();

        Bounds searchBounds = new Bounds(origin, new Vector3(MAX_SEARCH_DISTANCE, 1, MAX_SEARCH_DISTANCE));

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
            if (EnvironmentUtil.IsPositionFree(nodePosition))
            {
                ABPath path = ABPath.Construct(origin, nodePosition);

                path.heuristic = Heuristic.None;
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
                    if (!_inputData.RadiusMaps[rangeType].ContainsKey(nodePosition))
                    {
                        _inputData.RadiusMaps[rangeType].Add(nodePosition, cost);
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
    public bool FindSpawnLocationForCharacter(CharacterComponent character)
    {
        CharacterNavigator navigator = character.GetNavigator();

        //find a spawn point to place the character
        //see if we have a marker available to spawn them in

        if(character.GetTeam() == TeamID.Player)
        {
            List<GraphNode> SpawnNodes = EnvironmentUtil.GetNodesWithTag(TAG_LAYER_SPAWNLOCATION);

            if (SpawnNodes.Count > 0)
            {
                int index = Random.Range(0, SpawnNodes.Count - 1);
                GraphNode spawnNode = SpawnNodes[index];

                navigator.TeleportTo((Vector3)spawnNode.position);
                return true;
            }
        }

        GraphNode randomNNode;
        if (EnvironmentUtil.GetRandomUnoccupiedNode(out randomNNode))
        {
            Debug.Log("Placing character at random node");

            navigator.TeleportTo((Vector3)randomNNode.position);
            navigator.Rotate(Quaternion.identity);
            return true;
        }
        else
        {
            Debug.Log("Couldnt find location to place character");

        }

        return false;
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

    public Dictionary<Vector3, List<EnvironmentCoverData>> GetCoverMap()
    {
        return _coverMap;
    }
}

