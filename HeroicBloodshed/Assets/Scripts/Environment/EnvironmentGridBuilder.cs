using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using UnityEditor;

public class EnvironmentGridBuilder : MonoBehaviour
{
    [SerializeField] private GameObject TilePrefab;

    [SerializeField] private int Rows = 8;

    [SerializeField] private int Columns = 8;

    public void GenerateTiles()
    {
        GridGraph gridGraph = AstarPath.active.data.gridGraph;
        int index = 0;

        List<GraphNode> UnwalkableNodes = new List<GraphNode>();

        gridGraph.GetNodes(node =>
        {
            if (!node.Walkable)
            {
                UnwalkableNodes.Add(node);
            }

            return true;
        });

        Debug.Log("Found " + UnwalkableNodes.Count + " unwalkable nodes");

        gridGraph.GetNodes(node =>
        {
            Vector3 pos = ((Vector3)node.position);

            int col = (index % Columns);
            int row = (index / Columns);

            string tilename = "Tile " + (index+1) + " [ " + row + "," + col + " ] "; ;

            GameObject tileDecal = Instantiate<GameObject>(TilePrefab, pos, Quaternion.identity, this.transform);
            tileDecal.name = tilename;

            EnvironmentTile tile = tileDecal.GetComponent<EnvironmentTile>();

            Debug.Log(tilename);

            foreach(GraphNode unwalkableNode in UnwalkableNodes)
            {
                Bounds bounds = new Bounds(((Vector3)unwalkableNode.position), new Vector3(2, 2, 2));

                foreach(GraphNode neighbor in gridGraph.GetNodesInRegion(bounds))
                {
                    //if(((Vector3)neighbor.position) == tile.transform.position)
                    //{
                    //    tile.SetType(Constants.EnvironmentTileType.HalfCover);
                    //}
                }

            }

            index++;

            return true;
        });
    }
}

[CustomEditor(typeof(EnvironmentGridBuilder))]
public class EnvironmentGridBuilderEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EnvironmentGridBuilder builder = (EnvironmentGridBuilder)target;

        if (GUILayout.Button("Generate Tiles"))
        {

            builder.GenerateTiles();
        }

        if(GUILayout.Button("Clear"))
        {
            for (int i = 0; i < builder.transform.childCount; i++)
            {
                DestroyImmediate(builder.transform.GetChild(i).gameObject);
            }
        }
    }
}
