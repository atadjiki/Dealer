using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

[ExecuteInEditMode]
public class EnvironmentDebugGizmo : MonoBehaviour
{
    [Header("Settings")]
    public int GridSize = 8; //in multiples of unit size

    private EnvironmentTile[,] _tileGrid;

    private bool _scanComplete;

    [Header("Debug Settings")]
    [SerializeField] private bool ShowTraversibles = true;
    [SerializeField] private bool ShowNonTraversibles = false;
    [SerializeField] private bool ShowValidConnections = true;
    [SerializeField] private bool ShowInvalidConnections = false;
    [SerializeField] private bool ShowCover = true;

    private void OnDrawGizmosSelected()
    {
        if (_tileGrid != null && _scanComplete)
        {
            for (int Row = 0; Row < GridSize; Row++)
            {
                for (int Column = 0; Column < GridSize; Column++)
                {
                    EnvironmentTile tile = _tileGrid[Row, Column];

                    if (tile != null)
                    {
                        //draw a square for each tile 
                        if (ShowTraversibles || ShowNonTraversibles)
                        {
                            Color color = GetLayerDebugColor(tile.GetLayer(), ShowTraversibles, ShowNonTraversibles);
                            Gizmos.color = color;

                            Gizmos.DrawSphere(tile.GetOrigin(), 0.15f);

                            color.a = 0.1f;
                            Gizmos.color = color;
                            float width = ENV_TILE_SIZE * 0.95f;
                            Gizmos.DrawCube(tile.GetOrigin(), new Vector3(width, 0.1f, width));
                        }

                        EnvironmentTileConnectionMap neighborMap = EnvironmentUtil.GenerateNeighborMap(tile.GetOrigin());

                        //for each tile, draw the possible connections(i.e, is a move possible)
                        if (ShowValidConnections || ShowInvalidConnections)
                        {
                            if (IsLayerTraversible(tile.GetLayer()))
                            {
                                foreach (EnvironmentDirection dir in GetAllDirections())
                                {
                                    EnvironmentTileConnectionInfo info = neighborMap[dir];

                                    Vector3 direction = GetDirectionVector(dir) / 2;

                                    Gizmos.color = GetConnectionDebugColor(info, ShowValidConnections, ShowInvalidConnections);

                                    Gizmos.DrawLine(tile.GetOrigin() + new Vector3(0, 0.1f, 0), tile.GetOrigin() + new Vector3(0, 0.1f, 0) + direction);
                                }
                            }
                        }

                        if(ShowCover)
                        {
                            if (tile.GetLayer() != EnvironmentLayer.None)
                            {
                                foreach (EnvironmentDirection dir in GetCardinalDirections())
                                {
                                    EnvironmentTileConnectionInfo info = neighborMap[dir];

                                    if(info.IsObstructed)
                                    {
                                        Vector3[] edge = CalculateTileEdge(tile.GetOrigin(), dir);

                                        EnvironmentCover cover = GetCoverType(info);

                                        Gizmos.color = GetCoverDebugColor(cover);

                                        Gizmos.DrawLineList(edge);
                                    }
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

        _tileGrid = new EnvironmentTile[GridSize,GridSize];

        //gather all the nodes in the map
        for (int Row = 0; Row < GridSize; Row++)
        {
            for (int Column = 0; Column < GridSize; Column++)
            {
                //first check which tiles are walkable

                Vector3 origin = CalculateTileOrigin(Row, Column);

                EnvironmentLayer layer = EnvironmentUtil.CheckTileLayer(origin);

                _tileGrid[Row, Column] = new EnvironmentTile(Row, Column, layer);
            }
        }

        foreach(EnvironmentTile tile in _tileGrid)
        {
            List<Vector3> neighorVectors = EnvironmentUtil.GetTileNeighbors(tile.GetOrigin());

            foreach(Vector3 neighborVector in neighorVectors)
            {
                Vector2 coordinates = CalculateTileCoordinates(neighborVector);

                if(AreValidCoordinates(coordinates))
                {
                    EnvironmentTile neighbor = _tileGrid[(int)coordinates.x, (int)coordinates.y];

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

        if(coords.x >= GridSize || coords.y >= GridSize)
        {
            return false;
        }

        return true;
    }
}
