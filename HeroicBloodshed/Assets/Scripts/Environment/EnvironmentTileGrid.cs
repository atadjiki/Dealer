using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

public class EnvironmentTileGrid : MonoBehaviour, IEncounterEventHandler
{
    [SerializeField] private GameObject TilePrefab;
    [SerializeField] private GameObject PreviewPrefab;

    private Dictionary<GraphNode, EnvironmentTile> _tileMap;

    private List<GameObject> PreviewTiles;

    private bool _calculatingPath = false;

    private LineRenderer _pathRenderer;

    public IEnumerator Corutine_PerformSetup()
    {
        yield return Coroutine_GenerateTiles();
        yield return Coroutine_SetupRenderers();
    }

    private IEnumerator Coroutine_SetupRenderers()
    {
        Debug.Log("Setting Up Path Renderer");
        ResourceRequest pathRendererRequest = GetEnvironmentVFX(PrefabID.LineRenderer_Path);
        yield return new WaitUntil(() => pathRendererRequest.isDone);
        GameObject pathRendererObject = Instantiate<GameObject>((GameObject)pathRendererRequest.asset, this.transform);
        yield return new WaitWhile(() => pathRendererObject.GetComponent<LineRenderer>() == null);
        _pathRenderer = pathRendererObject.GetComponent<LineRenderer>();
    }

    private IEnumerator Coroutine_GenerateTiles()
    {
        Debug.Log("Generating Tiles");
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
            tile.OnTileHighlightState += OnTileHighlightState;

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
    }

    public IEnumerator Coroutine_EncounterStateUpdate(EncounterState stateID, EncounterModel model)
    {
        switch(stateID)
        {
            case EncounterState.CHOOSE_ACTION:
                if(!model.IsCurrentTeamCPU())
                {
                    AllowTileUpdate(true);
                    GenerateMovementRadius();
                }
                break;
            default:
                AllowTileUpdate(false);
                ClearLineRenderers();
                ClearRadiusTiles();
                break;
        }

        yield return null;
    }

    private void OnTileHighlightState(EnvironmentTile tile, bool highlighted)
    {
        if (_calculatingPath) { return; }
        if (tile.ContainsObstacle()) { return; }

        if (EncounterManager.Instance.GetCurrentState() == EncounterState.CHOOSE_ACTION
            && EncounterManager.Instance.IsPlayerTurn())
        {
            //if true, create a line renderer from the current character to this tile,
            if (highlighted)
            {
                GenerateMovementPath(tile);
            }

            //if false, clear line renderer
            else
            {
                _pathRenderer.positionCount = 0;
                _pathRenderer.forceRenderingOff = true;
            }
        }
    }

    private void GenerateMovementRadius()
    {
        PreviewTiles = new List<GameObject>();

        CharacterComponent currentCharacter = EncounterManager.Instance.GetCurrentCharacter();

        Vector3 origin = currentCharacter.GetWorldLocation();

        List<EnvironmentTile> eligibleTiles = new List<EnvironmentTile>();

        //find the distance between the character and every tile (yikes)
        foreach(GraphNode node in _tileMap.Keys)
        {
            ABPath path = ABPath.Construct(origin, ((Vector3)node.position));

            AstarPath.StartPath(path, true);

            path.BlockUntilCalculated();

            int length = path.vectorPath.Count;

            if (length < 12)
            {
                EnvironmentTile tile = GetClosestTile(node);
                if(tile.IsFree())
                {
                    eligibleTiles.Add(tile);
                }
            }
        }

        Debug.Log("Found " + eligibleTiles.Count + " eligible paths");

        foreach(EnvironmentTile tile in eligibleTiles)
        {
            Instantiate<GameObject>(PreviewPrefab, tile.transform);
        }
    }

    private void GenerateMovementPath(EnvironmentTile tile)
    {
        _calculatingPath = true;

        CharacterComponent currentCharacter = EncounterManager.Instance.GetCurrentCharacter();

        Vector3 origin = currentCharacter.GetWorldLocation();
        Vector3 destination = tile.transform.position;

        ABPath pendingPath = ABPath.Construct(origin, destination);

        AstarPath.StartPath(pendingPath, true);

        pendingPath.BlockUntilCalculated();

        int length = pendingPath.vectorPath.Count;

        if (length < 12)
        {
            _pathRenderer.positionCount = length;

            _pathRenderer.SetPositions(pendingPath.vectorPath.ToArray());

            _pathRenderer.forceRenderingOff = false;

        }

        _calculatingPath = false;
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

        if (tile != null)
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

    private void AllowTileUpdate(bool flag)
    {
        foreach(EnvironmentTile tile in _tileMap.Values)
        {
            tile.AllowUpdate(flag);
        }
    }

    private void ClearLineRenderers()
    {
        _pathRenderer.positionCount = 0;
        _pathRenderer.forceRenderingOff = true;
    }

    private void ClearRadiusTiles()
    {
        if(PreviewTiles != null)
        {
            foreach (GameObject radiusTile in PreviewTiles)
            {
                GameObject.Destroy(radiusTile);
            }
        }
    }

    private void OnTileSelected(EnvironmentTile tile)
    {
        EncounterManager.Instance.OnEnvironmentTileSelected(tile);
    }
}
