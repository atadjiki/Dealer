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

    [Header("Debug Settings")]
    [SerializeField] private bool DrawGizmos = true;
    [SerializeField] private bool ShowTraversibles = true;
    [SerializeField] private bool ShowNonTraversibles = false;
    [SerializeField] private bool ShowValidConnections = true;
    [SerializeField] private bool ShowInvalidConnections = false;
    [SerializeField] private bool ShowCover = true;

    private void OnDrawGizmosSelected()
    {
        if (DrawGizmos == false) return;

        for (int Row = 0; Row < GridSize; Row++)
        {
            for (int Column = 0; Column < GridSize; Column++)
            {
                Vector3 origin = CalculateTileOrigin(Row, Column);

                EnvironmentLayer layer = EnvironmentUtil.CheckTileLayer(origin);

                EnvironmentTile tile = new EnvironmentTile(Row, Column, layer);

                if (tile != null)
                {
                    //draw a square for each tile 
                    if (ShowTraversibles || ShowNonTraversibles)
                    {
                        Color color = GetLayerDebugColor(layer, ShowTraversibles, ShowNonTraversibles);
                        Gizmos.color = color;

                        Gizmos.DrawSphere(origin, 0.15f);

                        color.a = 0.1f;
                        Gizmos.color = color;
                        float width = ENV_TILE_SIZE * 0.95f;
                        Gizmos.DrawCube(origin, new Vector3(width, 0.1f, width));
                    }

                    EnvironmentTileConnectionMap neighborMap = EnvironmentUtil.GenerateNeighborMap(origin);

                    //for each tile, draw the possible connections(i.e, is a move possible)
                    if (ShowValidConnections || ShowInvalidConnections)
                    {
                        if (IsLayerTraversible(layer))
                        {
                            foreach (EnvironmentDirection dir in GetAllDirections())
                            {
                                EnvironmentTileConnectionInfo info = neighborMap[dir];

                                Vector3 direction = GetDirectionVector(dir) / 2;

                                Gizmos.color = GetConnectionDebugColor(info, ShowValidConnections, ShowInvalidConnections);

                                Gizmos.DrawLine(origin + new Vector3(0, 0.1f, 0), origin + new Vector3(0, 0.1f, 0) + direction);
                            }
                        }
                    }

                    if (ShowCover)
                    {
                        if (layer != EnvironmentLayer.NONE)
                        {
                            foreach (EnvironmentDirection dir in GetCardinalDirections())
                            {
                                EnvironmentTileConnectionInfo info = neighborMap[dir];

                                if (IsLayerCover(info.Obstruction))
                                {
                                    Vector3[] edge = CalculateTileEdge(origin, dir);

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
