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

    private EnvironmentTileGrid _tileGrid;

    private bool _ready = false;

    private LineRenderer _pathRenderer;

    private bool _calculatingPath = false;

    public bool IsEnvironmentReady() { return _ready; }

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

        StartCoroutine(Coroutine_Build());
    }

    private IEnumerator Coroutine_Build()
    {
        yield return Coroutine_ScanNavmesh();
        yield return Coroutine_BuildTileGrid();
        yield return Coroutine_SetupPathRenderer();

        //dispose of the setup navmesh after tiles are built
        GridGraph gridGraph = AstarPath.active.data.gridGraph;
        AstarPath.active.data.RemoveGraph(gridGraph);

        Debug.Log("Environment Ready");

        _ready = true;
    }

    private IEnumerator Coroutine_ScanNavmesh()
    {
        Debug.Log("Scanning navmesh");
        GridGraph gridGraph = AstarPath.active.data.gridGraph;
        AstarPath.active.Scan(gridGraph);

        yield return new WaitWhile(() => AstarPath.active.isScanning);
    }

    private IEnumerator Coroutine_BuildTileGrid()
    {
        Debug.Log("Building tile grid");
        yield return new WaitUntil(() => GetComponentInChildren<EnvironmentTileGrid>() != null);
        _tileGrid = GetComponentInChildren<EnvironmentTileGrid>();
        _tileGrid.GenerateTiles();
        yield return new WaitUntil(() => _tileGrid.IsGenerated());
    }

    private IEnumerator Coroutine_SetupPathRenderer()
    {
        Debug.Log("Setting Up Path Renderer");
        ResourceRequest resourceRequest = GetEnvironmentVFX(PrefabID.LineRenderer_Path);
        yield return new WaitUntil(() => resourceRequest.isDone);
        GameObject lineRendererObject = Instantiate<GameObject>((GameObject)resourceRequest.asset, this.transform);
        yield return new WaitWhile(() => lineRendererObject.GetComponent<LineRenderer>() == null);
        _pathRenderer = lineRendererObject.GetComponent<LineRenderer>();

        lineRendererObject.transform.position += new Vector3(0, 0.15f, 0);
    }

    //Tile events

    public void OnEnvironmentTileHighlightState(EnvironmentTile tile, bool highlighted)
    {
        if (_calculatingPath) { return; }
        if (tile.ContainsObstacle()) { return; }

        if(EncounterManager.Instance.GetCurrentState() == EncounterState.CHOOSE_ACTION
            && EncounterManager.Instance.IsPlayerTurn())
        {
            //if true, create a line renderer from the current character to this tile,
            if(highlighted)
            {
                _calculatingPath = true;

                CharacterComponent currentCharacter = EncounterManager.Instance.GetCurrentCharacter();

                Vector3 origin = currentCharacter.GetWorldLocation();
                Vector3 destination = tile.transform.position;

                ABPath pendingPath = ABPath.Construct(origin, destination);

                AstarPath.StartPath(pendingPath, true);

                pendingPath.BlockUntilCalculated();

                int length = pendingPath.vectorPath.Count;

                if(length < 12)
                {
                    _pathRenderer.positionCount = length;

                    _pathRenderer.SetPositions(pendingPath.vectorPath.ToArray());

                    _pathRenderer.forceRenderingOff = false;

                }

                _calculatingPath = false;

            }

            //if false, clear line renderer
            else
            {
                _pathRenderer.positionCount = 0;
                _pathRenderer.forceRenderingOff = true;
            }
        }
    }

    //Encounter Events

    public IEnumerator Coroutine_EncounterStateUpdate(EncounterState stateID, EncounterModel model)
    {
        if(_tileGrid != null)
        {
            yield return _tileGrid.Coroutine_EncounterStateUpdate(stateID, model);
        }

        if(stateID != EncounterState.CHOOSE_ACTION)
        {
            _pathRenderer.positionCount = 0;
            _pathRenderer.forceRenderingOff = true;
        }

        yield return null;
    }

    //Helpers/interface 

    public Vector3 GetClosestPositionToTile(EnvironmentTile tile)
    {
        return _tileGrid.GetClosestTilePosition(tile.transform.position);
    }

    public List<EnvironmentTile> GetTilesContainingSpawnPoints()
    {
        if (_tileGrid != null)
        {
            return _tileGrid.GetTilesContainingSpawnPoints();
        }

        return null;
    }
}
