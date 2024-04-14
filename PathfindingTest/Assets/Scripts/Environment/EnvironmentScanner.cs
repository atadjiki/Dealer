using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

[Serializable]
public class EnvironmentTile
{
    public EnvironmentLayer Layer;

    public Vector2 Coordinates;

    public EnvironmentTile(int _row, int _column, EnvironmentLayer _layer)
    {
        Coordinates = new Vector2(_row, _column);
        Layer = _layer;
    }

    public override string ToString()
    {
        return "Tile " + GetOrigin().ToString() + " - " + Layer.ToString();
    }

    public Vector3 GetOrigin()
    {
        return EnvironmentUtil.CalculateTileOrigin((int)Coordinates.x, (int)Coordinates.y);
    }
}

[ExecuteInEditMode]
public class EnvironmentScanner : MonoBehaviour
{
    [Header("Settings")]
    public int MapSize = 8; //in multiples of unit size

    private EnvironmentTile[,] _tileMap;

    private bool _scanComplete;

    private void OnDrawGizmosSelected()
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
                        Color color = EnvironmentUtil.GetLayerDebugColor(tile.Layer);
                        color.a = 0.25f;
                        Gizmos.color = color;

                        Gizmos.DrawSphere(tile.GetOrigin(), 0.1f);
                        Gizmos.DrawWireCube(tile.GetOrigin()
                            + new Vector3(0,ENV_TILE_SIZE/10), new Vector3(ENV_TILE_SIZE, ENV_TILE_SIZE/10, ENV_TILE_SIZE));

                        foreach (EnvironmentDirection dir in Enum.GetValues(typeof(EnvironmentDirection)))
                        {
                            Vector3 direction = EnvironmentUtil.GetGridDirection(dir);

                            Ray ray = new Ray(tile.GetOrigin(), direction);
                            Gizmos.DrawRay(ray);
                        }
                    }
                }
            }
        }
    }

    public void PerformScan()
    {
        StopAllCoroutines();
        StartCoroutine(Coroutine_PerformScan());
    }

    private IEnumerator Coroutine_PerformScan()
    {
        _scanComplete = false;

        _tileMap = new EnvironmentTile[MapSize,MapSize];

        //gather all the nodes in the map
        for (int Row = 0; Row < MapSize; Row++)
        {
            for (int Column = 0; Column < MapSize; Column++)
            {
                //first check which tiles are walkable

                Vector3 origin = EnvironmentUtil.CalculateTileOrigin(Row, Column);

                EnvironmentLayer layer = EnvironmentUtil.CheckTileLayer(origin);

                _tileMap[Row, Column] = new EnvironmentTile(Row, Column, layer);
            }
        }

        //determine the neighbors for each node in the map
        for (int Row = 0; Row < MapSize; Row++)
        {
            for (int Column = 0; Column < MapSize; Column++)
            {
                EnvironmentTile tile =_tileMap[Row,Column];
            }
        }

        _scanComplete = true;

        yield return null;
    }
}
