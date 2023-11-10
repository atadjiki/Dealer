using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

public class EnvironmentTileGrid : MonoBehaviour, IEncounterEventHandler
{
    [SerializeField] private GameObject TilePrefab;

    private Dictionary<GraphNode, EnvironmentTile> _tileMap;

    private bool _generated = false;

    public void GenerateTiles()
    {
        StartCoroutine(Coroutine_GenerateTiles());
    }

    public bool IsGenerated() { return _generated; }

    private IEnumerator Coroutine_GenerateTiles()
    {
        GridGraph gridGraph = AstarPath.active.data.gridGraph;
        List<GraphNode> WalkableNodes = new List<GraphNode>();

        //only create tiles for walkable nodes (including ones with obstacles on them)
        gridGraph.GetNodes(node =>
        {
            if (node.Walkable)
            {
                WalkableNodes.Add(node);
            }

            return true;
        });

        _tileMap = new Dictionary<GraphNode, EnvironmentTile>();

        int index = 0;

        //generate a tile for each walkable node
        foreach (GraphNode node in WalkableNodes)
        {
            Vector3 pos = ((Vector3)node.position);

            int row = (index / gridGraph.Width);
            int col = (index % gridGraph.Width);

            string tilename = "Tile " + (index + 1) + " [ " + row + "," + col + " ] "; ;

            GameObject tileDecal = Instantiate<GameObject>(TilePrefab, pos, Quaternion.identity, this.transform);
            tileDecal.name = tilename;

            yield return new WaitWhile(() => tileDecal.GetComponent<EnvironmentTile>() == null);

            EnvironmentTile tile = tileDecal.GetComponent<EnvironmentTile>();
            tile.Setup(row, col);
            tile.OnTileSelected += OnTileSelected;

            _tileMap.Add(node, tile);

            index++;
        }

        //once we have our tiles, check if they are under any obstacles, and if they have any neighboring nodes
        foreach(EnvironmentTile tile in _tileMap.Values)
        {
            tile.PerformScan();
        }

        foreach(EnvironmentTile tile in _tileMap.Values)
        {
            tile.PerformCoverCheck();
        }

        _generated = true;
    }

    public void OnTileSelected(EnvironmentTile tile)
    {
        EncounterManager.Instance.OnEnvironmentTileSelected(tile);
    }

    public EnvironmentTile GetClosestTile(GraphNode node)
    {
        if (_tileMap.ContainsKey(node))
        {
            return _tileMap[node];
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

    public Vector3 GetClosestTilePosition(Vector3 worldPosition)
    {
        EnvironmentTile tile = GetClosestTile(worldPosition);

        if(tile != null)
        {
            return tile.transform.position;
        }
        else
        {
            Debug.Log("Could not find tile close to position " + worldPosition.ToString());
            return worldPosition;
        }
    }

    public List<EnvironmentTile> GetTilesContainingSpawnPoints(TeamID team = TeamID.None)
    {
        List<EnvironmentTile> tiles = new List<EnvironmentTile>();

        foreach (EnvironmentTile tile in _tileMap.Values)
        {
            if(tile.ContainsSpawnPoint())
            {
                EnvironmentSpawnPoint spawnPoint = tile.GetSpawnPoint();

                if(spawnPoint.GetTeam() == team || team == TeamID.None)
                {
                    tiles.Add(tile);
                }
            }
        }

        Debug.Log("Found " + tiles.Count + " spawn points");

        return tiles;
    }

    public List<EnvironmentTile> GetTilesContainingObstacles()
    {
        List<EnvironmentTile> tiles = new List<EnvironmentTile>();

        foreach (EnvironmentTile tile in _tileMap.Values)
        {
            if (tile.ContainsObstacle())
            {
                tiles.Add(tile);
            }
        }

        return tiles;
    }

    public IEnumerator Coroutine_EncounterStateUpdate(EncounterState stateID, EncounterModel model)
    {
        switch(stateID)
        {
            case EncounterState.CHOOSE_ACTION:

                if(!model.IsCurrentTeamCPU())
                {
                    AllowTileUpdate(true);
                }

                break;
            default:
                AllowTileUpdate(false);
                break;
        }

        yield return null;
    }

    private void AllowTileUpdate(bool flag)
    {
        foreach(EnvironmentTile tile in _tileMap.Values)
        {
            tile.AllowUpdate(flag);
        }
    }
}
