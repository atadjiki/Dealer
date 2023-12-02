using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static Constants;

public class EnvironmentTileGrid : MonoBehaviour, IEncounterEventHandler
{
    [SerializeField] private GameObject TilePrefab;

    private Dictionary<Vector3, EnvironmentTile> _tileMap;

    private bool _calculatingPath = false;

    private EnvironmentTile _currentlyHighlighted;

    public IEnumerator Corutine_PerformSetup()
    {
        yield return null;
    }

    public IEnumerator Coroutine_EncounterStateUpdate(EncounterState stateID, EncounterModel model)
    {
        switch(stateID)
        {
            case EncounterState.CHOOSE_ACTION:
                if(!model.IsCurrentTeamCPU())
                {
                    // yield return GenerateMovementRadius();
                    yield return null;
                }
                break;
            default:
                break;
        }

        yield return null;
    }

    private IEnumerator GenerateMovementRadius()
    {
        CharacterComponent currentCharacter = EncounterManager.Instance.GetCurrentCharacter();

        Vector3 origin = currentCharacter.GetWorldLocation();

        List<Tuple<Vector3, int>> eligibleTiles = new List<Tuple<Vector3, int>>();

        //find the distance between the character and every tile (yikes)
        foreach(Vector3 mapNode in _tileMap.Keys)
        {
            if(_tileMap[mapNode].IsFree())
            {
                ABPath path = ABPath.Construct(origin, mapNode);

                AstarPath.StartPath(path, true);

                yield return new WaitUntil(() => path.CompleteState == PathCompleteState.Complete);

                int cost = 0;

                foreach (GraphNode pathNode in path.path)
                {
                    cost += (int)path.GetTraversalCost(pathNode);
                }

                MovementRangeType rangeType;
                if (EnvironmentUtil.IsWithinCharacterRange(cost, currentCharacter, out rangeType))
                {
                    EnvironmentTile tile = GetClosestTile(mapNode);
                    if (tile.IsFree())
                    {
                        eligibleTiles.Add(new Tuple<Vector3, int>(mapNode, cost));
                    }
                }
            }
        }

        Debug.Log("Found " + eligibleTiles.Count + " eligible paths");

        foreach (Tuple<Vector3, int> pair in eligibleTiles)
        {
            EnvironmentTile tile = _tileMap[pair.Item1];

            if (pair.Item2 <= currentCharacter.GetMovementRange())
            {
                tile.SetPreviewState(EnvironmentTilePreviewState.Half);
            }
            else
            {
                tile.SetPreviewState(EnvironmentTilePreviewState.Full);
            }
        }
    }

    public EnvironmentTile GetClosestTile(GraphNode node)
    {
        if (_tileMap.ContainsKey((Vector3)node.position))
        {
            return _tileMap[(Vector3)node.position];
        }
        else
        {
            return null;
        }
    }

    public EnvironmentTile GetClosestTile(Vector3 worldPosition)
    {
        GridGraph gridGraph = AstarPath.active.data.gridGraph;
        if (gridGraph != null)
        {
            NNInfoInternal nodeInfo = gridGraph.GetNearest(worldPosition);

            return GetClosestTile(nodeInfo.node);
        }
        else
        {
            Debug.Log("Could not find node close to " + worldPosition.ToString());
            return null;
        }
    }

    public List<EnvironmentTile> GetTilesContainingSpawnPoints(TeamID team = TeamID.None)
    {
        List<EnvironmentTile> tiles = new List<EnvironmentTile>();

        if (_tileMap != null)
        {
            foreach (EnvironmentTile tile in _tileMap.Values)
            {
                if (tile.ContainsSpawnPoint())
                {
                    EnvironmentSpawnPoint spawnPoint = tile.GetSpawnPoint();

                    if (spawnPoint != null)
                    {
                        if (spawnPoint.GetTeam() == team || team == TeamID.None)
                        {
                            tiles.Add(tile);
                        }
                    }
                }
            }
        }

        Debug.Log("Found " + tiles.Count + " spawn points for team " + team);

        return tiles;
    }
}
