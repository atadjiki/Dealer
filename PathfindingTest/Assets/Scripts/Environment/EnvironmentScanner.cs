using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

[Serializable]
public class EnvironmentTile
{
    public EnvironmentLayer Layer;

    public Vector3 Origin;

    public EnvironmentTile(Vector3 _origin, EnvironmentLayer _layer)
    {
        Origin = _origin;
        Layer = _layer;
    }

    public override string ToString()
    {
        return "Tile " + Origin.ToString() + " - " + Layer.ToString();
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
            for (int Row = 0; Row < MapSize; Row++)
            {
                for (int Column = 0; Column < MapSize; Column++)
                {
                    EnvironmentTile tile = _tileMap[Row, Column];

                    if (tile != null)
                    {
                        Debug.DrawRay(tile.Origin, Vector3.up, EnvironmentUtil.GetLayerDebugColor(tile.Layer), Time.deltaTime, false);
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

                Vector3 origin = EnvironmentUtil.CalculateTileOrigin(rows, columns);

                EnvironmentLayer layer = EnvironmentUtil.CheckTileLayer(origin);


                _tileMap[rows, columns] = new EnvironmentTile(origin, layer);

                //for each walkable tile, scan in 4/6 directions to see the status of their neighbors

                yield return new WaitForFixedUpdate();
            }
        }

        _scanComplete = true;

        yield return null;
    }
}
