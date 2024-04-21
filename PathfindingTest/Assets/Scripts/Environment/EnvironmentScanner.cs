using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

[SerializeField]
public class EnvironmentTileNeighborMap : Dictionary<EnvironmentDirection, bool>
{
    public EnvironmentTileNeighborMap()
    {
        foreach(EnvironmentDirection dir in EnvironmentUtil.GetAllDirections())
        {
            Add(dir, false);
        }
    }
}

[Serializable]
public class EnvironmentTile
{
    public EnvironmentLayer Layer;

    public Vector2 Coordinates;

    public EnvironmentTileNeighborMap _neighborMap;

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

    public EnvironmentTileNeighborMap GetNeighborMap()
    {
        return _neighborMap;
    }

    public void RefreshNeighborMap()
    {
        _neighborMap = EnvironmentUtil.GenerateNeighborMap(GetOrigin());
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

                        if(EnvironmentUtil.IsLayerTraversible(tile.Layer))
                        {
                            EnvironmentTileNeighborMap neighborMap = tile.GetNeighborMap();

                            foreach (EnvironmentDirection dir in EnvironmentUtil.GetAllDirections())
                            {
                                Vector3 direction = EnvironmentUtil.GetDirectionVector(dir)/2;

                                Gizmos.color = EnvironmentUtil.GetConnectionDebugColor(neighborMap[dir]);

                                Vector3 offset = new Vector3(0, 0.1f, 0);

                                Gizmos.DrawLine(tile.GetOrigin() + offset, tile.GetOrigin() + offset + direction);
                            }
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

        foreach(EnvironmentTile tile in _tileMap)
        {
            tile.RefreshNeighborMap();
        }

        _scanComplete = true;

        yield return null;
    }
}
