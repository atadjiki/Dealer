using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static Constants;

public class EnvironmentTileGrid : MonoBehaviour, IEncounterEventHandler
{
    [SerializeField] private GameObject TilePrefab;

    private Dictionary<Vector3, EnvironmentTile> _tileMap;

    private bool _calculatingPath = false;

    private LineRenderer _pathRenderer;

    private EnvironmentTile _currentlyHighlighted;

    private EnvironmentTileActiveState _activeState = EnvironmentTileActiveState.Inactive;

    public IEnumerator Corutine_PerformSetup()
    {
        yield return Coroutine_GenerateTiles();
        yield return Coroutine_SetupRenderers();
    }

    private IEnumerator Coroutine_SetupRenderers()
    {
        Debug.Log("Setting Up Path Renderer");
        ResourceRequest pathRendererRequest = GetEnvironmentVFX(PrefabID.LineRenderer_Path);
        yield return new WaitUntil(() => pathRendererRequest.isDone);
        GameObject pathRendererObject = Instantiate<GameObject>((GameObject)pathRendererRequest.asset, this.transform);
        yield return new WaitWhile(() => pathRendererObject.GetComponent<LineRenderer>() == null);
        _pathRenderer = pathRendererObject.GetComponent<LineRenderer>();
    }

    private IEnumerator Coroutine_GenerateTiles()
    {
        Debug.Log("Generating Tiles");
        GridGraph gridGraph = AstarPath.active.data.gridGraph;
        List<GraphNode> WalkableNodes = new List<GraphNode>();

        //only create tiles for walkable nodes (including ones with obstacles on them)
        gridGraph.GetNodes(node =>
        {
            if (node.Walkable)
            {
                WalkableNodes.Add(node);
            }

            return true;
        });

        _tileMap = new Dictionary<Vector3, EnvironmentTile>();

        int index = 0;

        float loadTime = Time.time;

        //generate a tile for each walkable node
        foreach (GraphNode node in WalkableNodes)
        {
            Vector3 pos = ((Vector3)node.position);

            int row = (index / gridGraph.Width);
            int col = (index % gridGraph.Width);

            string tilename = "Tile " + (index + 1) + " [ " + row + "," + col + " ] "; ;

            GameObject tileObject = Instantiate<GameObject>(TilePrefab, pos, Quaternion.identity, this.transform);
            tileObject.name = tilename;
            yield return new WaitWhile(() => tileObject.GetComponent<EnvironmentTile>() == null);
            EnvironmentTile tile = tileObject.GetComponent<EnvironmentTile>();
            _tileMap.Add((Vector3) node.position, tile);
            yield return new WaitForEndOfFrame();

            index++;
        }

        loadTime -= Time.time;

        Debug.Log(Mathf.Abs(loadTime) + " seconds to generate tiles");

        //once we have our tiles, check if they are under any obstacles, and if they have any neighboring nodes
        foreach(EnvironmentTile tile in _tileMap.Values)
        {
            tile.PerformScan();
        }
    }

    public IEnumerator Coroutine_EncounterStateUpdate(EncounterState stateID, EncounterModel model)
    {
        switch(stateID)
        {
            case EncounterState.CHOOSE_ACTION:
                if(!model.IsCurrentTeamCPU())
                {
                    SetActiveState(EnvironmentTileActiveState.Active);
                    yield return GenerateMovementRadius();
                }
                break;
            default:
                SetActiveState(EnvironmentTileActiveState.Inactive);
                ClearLineRenderers();
                break;
        }

        yield return null;
    }

    private void Update()
    {
        if (_activeState == EnvironmentTileActiveState.Active)
        {
            CheckMouseHighlight();
            CheckMouseClick();
        }
    }

    private void CheckMouseHighlight()
    {
        if (CheckIsMouseBlocked()) { return; }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100, LayerMask.GetMask("EnvironmentTile")))
        {
            EnvironmentTile hitTile = hit.collider.GetComponent<EnvironmentTile>();

            if (hitTile != null && _currentlyHighlighted != hitTile)
            {
                hitTile.SetHighlightState(EnvironmentTileHighlightState.On);

                HighlightTile(hitTile);

                return;
            }
        }
    }

    private void HighlightTile(EnvironmentTile tile)
    {
        if(_currentlyHighlighted != null)
        {
            //un highlight the old tile
            _currentlyHighlighted.SetHighlightState(EnvironmentTileHighlightState.Off);

        }

        _currentlyHighlighted = tile;

        //highlight the new tile
        _currentlyHighlighted.SetHighlightState(EnvironmentTileHighlightState.On);

        ClearLineRenderers();

        StartCoroutine(GenerateMovementPath(_currentlyHighlighted));
    }

    private void CheckMouseClick()
    {
        if (CheckIsMouseBlocked()) { return; }

        if (!Input.GetMouseButtonDown(0)) return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100, LayerMask.GetMask("EnvironmentTile")))
        {
            EnvironmentTile tile = hit.collider.GetComponent<EnvironmentTile>();

            StartCoroutine(Coroutine_OnTileSelected(tile));
        }
    }
    private IEnumerator Coroutine_OnTileSelected(EnvironmentTile tile)
    {
        CharacterComponent currentCharacter = EncounterManager.Instance.GetCurrentCharacter();

        int movementRange = currentCharacter.GetMovementRange();

        Vector3 origin = currentCharacter.GetWorldLocation();

        ABPath path = ABPath.Construct(origin, tile.transform.position);

        AstarPath.StartPath(path, true);

        yield return new WaitUntil(() => path.CompleteState == PathCompleteState.Complete);

        int cost = 0;

        foreach (GraphNode pathNode in path.path)
        {
            cost += (int)path.GetTraversalCost(pathNode);
        }

        Debug.Log("Path cost is " + cost);

        MovementRangeType rangeType;
        if (IsWithinCharacterRange(cost, currentCharacter, out rangeType))
        {
            EncounterManager.Instance.OnEnvironmentTileSelected(tile, rangeType);
        }
        else
        {
            Debug.Log(tile.name + " is out of range for " + currentCharacter.GetID());
        }
    }

    private IEnumerator GenerateMovementPath(EnvironmentTile tile)
    {
        _calculatingPath = true;

        CharacterComponent currentCharacter = EncounterManager.Instance.GetCurrentCharacter();

        int movementRange = currentCharacter.GetMovementRange();

        Vector3 origin = currentCharacter.GetWorldLocation();
        Vector3 destination = tile.transform.position;

        ABPath pendingPath = ABPath.Construct(origin, destination);

        AstarPath.StartPath(pendingPath, true);

        yield return new WaitUntil(() => pendingPath.CompleteState == PathCompleteState.Complete);

        List<Vector3> vectorPath = pendingPath.vectorPath;

        int length = vectorPath.Count;

        vectorPath.Add(destination);

        MovementRangeType rangeType;

        if (IsWithinCharacterRange(length, currentCharacter, out rangeType))
        {
            _pathRenderer.positionCount = vectorPath.Count;

            _pathRenderer.SetPositions(vectorPath.ToArray());

            Color pathColor = GetColor(rangeType);
            pathColor.a = 0.25f;

            _pathRenderer.material.color = pathColor;

            _pathRenderer.forceRenderingOff = false;
        }

        _calculatingPath = false;
    }


    private IEnumerator GenerateMovementRadius()
    {
        CharacterComponent currentCharacter = EncounterManager.Instance.GetCurrentCharacter();

        Vector3 origin = currentCharacter.GetWorldLocation();

        List<Tuple<Vector3, int>> eligibleTiles = new List<Tuple<Vector3, int>>();

        //find the distance between the character and every tile (yikes)
        foreach(Vector3 mapNode in _tileMap.Keys)
        {
            if(_tileMap[mapNode].IsFree())
            {
                ABPath path = ABPath.Construct(origin, mapNode);

                AstarPath.StartPath(path, true);

                yield return new WaitUntil(() => path.CompleteState == PathCompleteState.Complete);

                int cost = 0;

                foreach (GraphNode pathNode in path.path)
                {
                    cost += (int)path.GetTraversalCost(pathNode);
                }

                MovementRangeType rangeType;
                if (IsWithinCharacterRange(cost, currentCharacter, out rangeType))
                {
                    EnvironmentTile tile = GetClosestTile(mapNode);
                    if (tile.IsFree())
                    {
                        eligibleTiles.Add(new Tuple<Vector3, int>(mapNode, cost));
                    }
                }
            }
        }

        Debug.Log("Found " + eligibleTiles.Count + " eligible paths");

        foreach (Tuple<Vector3, int> pair in eligibleTiles)
        {
            EnvironmentTile tile = _tileMap[pair.Item1];

            if (pair.Item2 <= currentCharacter.GetMovementRange())
            {
                tile.SetPreviewState(EnvironmentTilePreviewState.Half);
            }
            else
            {
                tile.SetPreviewState(EnvironmentTilePreviewState.Full);
            }
        }
    }

    public EnvironmentTile GetClosestTile(GraphNode node)
    {
        if (_tileMap.ContainsKey((Vector3)node.position))
        {
            return _tileMap[(Vector3)node.position];
        }
        else
        {
            return null;
        }
    }

    public EnvironmentTile GetClosestTile(Vector3 worldPosition)
    {
        GridGraph gridGraph = AstarPath.active.data.gridGraph;
        if (gridGraph != null)
        {
            NNInfoInternal nodeInfo = gridGraph.GetNearest(worldPosition);

            return GetClosestTile(nodeInfo.node);
        }
        else
        {
            Debug.Log("Could not find node close to " + worldPosition.ToString());
            return null;
        }
    }

    public Vector3 GetClosestTilePosition(Vector3 worldPosition)
    {
        EnvironmentTile tile = GetClosestTile(worldPosition);

        if (tile != null)
        {
            return tile.transform.position;
        }
        else
        {
            Debug.Log("Could not find tile close to position " + worldPosition.ToString());
            return worldPosition;
        }
    }

    public List<EnvironmentTile> GetTilesContainingSpawnPoints(TeamID team = TeamID.None)
    {
        List<EnvironmentTile> tiles = new List<EnvironmentTile>();

        if (_tileMap != null)
        {
            foreach (EnvironmentTile tile in _tileMap.Values)
            {
                if (tile.ContainsSpawnPoint())
                {
                    EnvironmentSpawnPoint spawnPoint = tile.GetSpawnPoint();

                    if (spawnPoint != null)
                    {
                        if (spawnPoint.GetTeam() == team || team == TeamID.None)
                        {
                            tiles.Add(tile);
                        }
                    }
                }
            }
        }

        Debug.Log("Found " + tiles.Count + " spawn points for team " + team);

        return tiles;
    }

    public List<EnvironmentTile> GetTilesContainingObstacles()
    {
        List<EnvironmentTile> tiles = new List<EnvironmentTile>();

        foreach (EnvironmentTile tile in _tileMap.Values)
        {
            if (tile.ContainsObstacle())
            {
                tiles.Add(tile);
            }
        }

        return tiles;
    }

    public void SetActiveState(EnvironmentTileActiveState state)
    {
        _activeState = state;

        if (_activeState == EnvironmentTileActiveState.Active)
        {

        }
        else
        {
            foreach (EnvironmentTile tile in _tileMap.Values)
            {
                tile.SetHighlightState(EnvironmentTileHighlightState.Off);
                tile.SetPreviewState(EnvironmentTilePreviewState.None);
            }
        }
    }

    private void ClearLineRenderers()
    {
        _pathRenderer.positionCount = 0;
        _pathRenderer.forceRenderingOff = true;
    }

    private bool CheckIsMouseBlocked()
    {
        if (Camera.main == null)
        {
            return true;
        }

        if (EventSystem.current.IsPointerOverGameObject())
        {
            return true;
        }

        return false;
    }

    private bool IsWithinCharacterRange(int distance, CharacterComponent character, out MovementRangeType rangeType)
    {
        int threshold;

        int AP = character.GetActionPoints();
        int movementRange = character.GetMovementRange();

        if (AP >= GetAbilityCost(AbilityID.MoveFull))
        {
            threshold = (movementRange * 2);
        }
        else
        {
            threshold = movementRange;
        }

        if(distance <= threshold)
        {
            if(distance <= character.GetMovementRange())
            {
                rangeType = MovementRangeType.Half;
            }
            else
            {
                rangeType = MovementRangeType.Full;
            }

            return true;
        }

        rangeType = MovementRangeType.None;
        return false;
    }
}
