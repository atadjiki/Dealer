using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using UnityEditor;

public class EnvironmentGridBuilder : MonoBehaviour
{
    [SerializeField] private GameObject TilePrefab;

    public void GenerateTiles()
    {
        GridGraph gridGraph = AstarPath.active.data.gridGraph;

        List<GraphNode> WalkableNodes = new List<GraphNode>();

        gridGraph.GetNodes(node =>
        {
            if(node.Walkable)
            {
                WalkableNodes.Add(node);


            }

            return true;
        });

        StartCoroutine(Coroutine_GenerateTiles(WalkableNodes));
    }

    private IEnumerator Coroutine_GenerateTiles(List<GraphNode> WalkableNodes)
    {
        GridGraph gridGraph = AstarPath.active.data.gridGraph;
        int index = 0;

        foreach (GraphNode node in WalkableNodes)
        {
            Vector3 pos = ((Vector3)node.position);

            int row = (index / gridGraph.Width);
            int col = (index % gridGraph.Width);

            string tilename = "Tile " + (index + 1) + " [ " + row + "," + col + " ] "; ;

            GameObject tileDecal = Instantiate<GameObject>(TilePrefab, pos, Quaternion.identity, this.transform);
            tileDecal.name = tilename;

            yield return new WaitUntil(() => tileDecal.GetComponent<EnvironmentTile>() != null);

            EnvironmentTile tile = tileDecal.GetComponent<EnvironmentTile>();
            tile.Setup(row, col);

            Debug.Log(tilename);

            index++;
        }
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
