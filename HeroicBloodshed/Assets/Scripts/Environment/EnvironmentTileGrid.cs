using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentTileGrid : MonoBehaviour
{
    public delegate void TilesGeneratedDelegate();

    public TilesGeneratedDelegate OnTilesGenerated;

    [SerializeField] private GameObject TilePrefab;

    private Dictionary<GraphNode, EnvironmentTile> _tileMap;

    public void GenerateTiles()
    {
        StartCoroutine(Coroutine_GenerateTiles());
    }

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

            _tileMap.Add(node, tile);

            index++;
        }

        if(OnTilesGenerated != null)
        {
            OnTilesGenerated.Invoke();
        }
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
}
