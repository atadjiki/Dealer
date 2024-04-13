using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

[Serializable]
public class EnvironmentTile
{
    public EnvironmentTileState State;

    public Vector3 Center;

    public List<EnvironmentTile> Neighbors;

    public EnvironmentTile(Vector3 _center, EnvironmentTileState _state)
    {
        Center = _center;
        State = _state;
    }

    public override string ToString()
    {
        return "Tile " + Center.ToString() + " - " + State.ToString();
    }
}

public class EnvironmentScanner : MonoBehaviour
{
    [Header("Settings")]
    public int MapSize = 8; //in multiples of unit size

    private EnvironmentTile[,] _tileMap;

    private bool _scanComplete;

    private void Awake()
    {
        PerformScan();
    }

    private void Update()
    {
        if (_tileMap != null && _scanComplete)
        {
            for (int rows = 0; rows < MapSize; rows++)
            {
                for (int columns = 0; columns < MapSize; columns++)
                {
                    EnvironmentTile tile = _tileMap[rows, columns];

                    if (tile != null)
                    {
                        Debug.DrawRay(tile.Center, Vector3.up, EnvironmentUtil.GetTileStateColor(tile.State), Time.deltaTime, false);
                    }
                }
            }
        }
    }

    private void PerformScan()
    {
        StartCoroutine(Coroutine_PerformScan());
    }

    private IEnumerator Coroutine_PerformScan()
    {
        _tileMap = new EnvironmentTile[MapSize,MapSize];

        for (int rows = 0; rows < MapSize; rows++)
        {
            for (int columns = 0; columns < MapSize; columns++)
            {
                //first check which tiles are walkable

                Vector3 center = EnvironmentUtil.CalculateTileCenter(rows, columns);

                EnvironmentTileState tileState = EnvironmentUtil.ScanForHit(center);

                _tileMap[rows, columns] = new EnvironmentTile(center, tileState);

                //for each walkable tile, scan in 4/6 directions to see the status of their neighbors

                yield return new WaitForFixedUpdate();
            }
        }

        _scanComplete = true;

        yield return null;
    }

    private void PerformNeighborCheck(Transform nodeTransform, Vector3 direction)
    {
        Vector3 origin = nodeTransform.position + new Vector3(ENV_TILE_SIZE / 2, 0.1f, ENV_TILE_SIZE / 2); ;

        bool obstacleHalfHit = Physics.Raycast(origin, direction, ENV_TILE_SIZE, LAYER_OBSTACLE_HALF);
        bool obstacleFullHit = Physics.Raycast(origin, direction, ENV_TILE_SIZE, LAYER_OBSTACLE_FULL);
    }


}
