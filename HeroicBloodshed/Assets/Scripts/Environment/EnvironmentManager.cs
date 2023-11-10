using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System;
using static Constants;

public class EnvironmentManager: MonoBehaviour
{
    //singleton
    private static EnvironmentManager _instance;
    public static EnvironmentManager Instance { get { return _instance; } }

    private EnvironmentTileGrid _tileGrid;

    private bool _ready = false;

    public bool IsEnvironmentReady() { return _ready; }

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

    public Vector3 GetClosestPositionToTile(EnvironmentTile tile)
    {
        return _tileGrid.GetClosestTilePosition(tile.transform.position);
    }

    public CharacterComponent SpawnCharacter(TeamID teamID, CharacterID characterID)
    {
        //see if we have a marker available to spawn them in
        foreach (EnvironmentTile tile in _tileGrid.GetTilesContainingSpawnPoints(teamID))
        {
            GameObject characterObject = EnvironmentUtil.CreateCharacterObject(teamID + "_" + characterID, tile);
            CharacterComponent characterComponent = EnvironmentUtil.AddComponentByTeam(characterID, characterObject);

            return characterComponent;
        }

        return null;
    }
}
