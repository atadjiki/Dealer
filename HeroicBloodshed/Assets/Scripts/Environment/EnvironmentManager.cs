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
        yield return Coroutine_BuildTileGrid();

        //dispose of the setup navmesh after tiles are built
        GridGraph gridGraph = AstarPath.active.data.gridGraph;
        AstarPath.active.data.RemoveGraph(gridGraph);

        Debug.Log("Environment Ready");
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
        _tileGrid = GetComponentInChildren<EnvironmentTileGrid>();
        yield return _tileGrid.Corutine_PerformSetup();
    }

    //Encounter Events

    public IEnumerator Coroutine_EncounterStateUpdate(EncounterState stateID, EncounterModel model)
    {
        if (_tileGrid != null)
        {
            yield return _tileGrid.Coroutine_EncounterStateUpdate(stateID, model);
        }

        yield return null;
    }

    //Helpers/interface 

    public Vector3 GetClosestPositionToTile(EnvironmentTile tile)
    {
        return _tileGrid.GetClosestTilePosition(tile.transform.position);
    }

    public List<EnvironmentTile> GetTilesContainingSpawnPoints(TeamID teamID)
    {
        if (_tileGrid != null)
        {
            return _tileGrid.GetTilesContainingSpawnPoints(teamID);
        }

        return null;
    }
}
