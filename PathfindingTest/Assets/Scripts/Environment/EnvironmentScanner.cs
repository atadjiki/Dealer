using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

[ExecuteInEditMode]
public class EnvironmentScanner : MonoBehaviour
{
    [Header("Settings")]
    public int MapSize = 8; //in multiples of unit size

    private EnvironmentTile[,] _tileMap;

    private bool _scanComplete;

    [Header("Debug Settings")]
    [SerializeField] private bool Debug_ShowTileLayers;
    [SerializeField] private bool Debug_ShowTileConnections;
    [SerializeField] private bool Debug_ShowNonWalkableNodes;
    [SerializeField] private bool Debug_ShowInvalidConnections;

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
                        if (Debug_ShowTileLayers)
                        {
                            Color color = EnvironmentUtil.GetLayerDebugColor(tile.GetLayer(), Debug_ShowNonWalkableNodes);
                            Gizmos.color = color;

                            Gizmos.DrawSphere(tile.GetOrigin(), 0.15f);

                            color.a = 0.1f;
                            Gizmos.color = color;
                            float width = ENV_TILE_SIZE * 0.95f;
                            Gizmos.DrawCube(tile.GetOrigin(), new Vector3(width, 0.1f, width));
                        }

                        if(Debug_ShowTileConnections)
                        {
                            if (EnvironmentUtil.IsLayerTraversible(tile.GetLayer()))
                            {
                                EnvironmentTileConnectionMap neighborMap = EnvironmentUtil.GenerateNeighborMap(tile.GetOrigin());

                                foreach (EnvironmentDirection dir in EnvironmentUtil.GetAllDirections())
                                {
                                    bool valid = neighborMap[dir];

                                    Vector3 direction = EnvironmentUtil.GetDirectionVector(dir) / 2;

                                    Gizmos.color = EnvironmentUtil.GetConnectionDebugColor(valid, Debug_ShowInvalidConnections);

                                    Vector3 offset = new Vector3(0, 0.1f, 0);

                                    Gizmos.DrawLine(tile.GetOrigin() + offset, tile.GetOrigin() + offset + direction);
                                }
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
            List<Vector3> neighorVectors = EnvironmentUtil.GetTileNeighbors(tile.GetOrigin());

            foreach(Vector3 neighborVector in neighorVectors)
            {
                Vector2 coordinates = EnvironmentUtil.CalculateTileCoordinates(neighborVector);

                if(AreValidCoordinates(coordinates))
                {
                    EnvironmentTile neighbor = _tileMap[(int)coordinates.x, (int)coordinates.y];

                    tile.AddNeighbor(neighbor);
                }
            }
        }

        _scanComplete = true;

        yield return null;
    }

    public bool AreValidCoordinates(Vector2 coords)
    {
        if(coords.x < 0 || coords.y < 0)
        {
            return false;
        }

        if(coords.x >= MapSize || coords.y >= MapSize)
        {
            return false;
        }

        return true;
    }
}
