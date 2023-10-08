using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using UnityEditor;

public class EncounterGridVisualizer : MonoBehaviour
{
    [SerializeField] private GameObject TilePrefab;

    [SerializeField] private int Rows = 8;

    [SerializeField] private int Columns = 8;

    public void GenerateTiles()
    {
        GridGraph gridGraph = AstarPath.active.data.gridGraph;
        int index = 0;

        gridGraph.GetNodes(node =>
        {
            Debug.Log("Node " + index + " " + node.position.ToString());

            int col = (index % Columns);
            int row = (index / Columns);

            Vector3 pos = ((Vector3)node.position);

            GameObject tileDecal = Instantiate<GameObject>(TilePrefab, pos, Quaternion.identity, this.transform);
            tileDecal.name = "Node " + index + " [ " + row + "," + col + " ] ";

            index++;

            return true;
        });
    }
}

[CustomEditor(typeof(EncounterGridVisualizer))]
public class EncounterTileBuilder : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Generate Tiles"))
        {
            EncounterGridVisualizer gridVisualizer = (EncounterGridVisualizer)target;
            gridVisualizer.GenerateTiles();
        }

        if(GUILayout.Button("Clear"))
        {
            EncounterGridVisualizer gridVisualizer = (EncounterGridVisualizer)target;

            for (int i = 0; i < gridVisualizer.transform.childCount; i++)
            {
                DestroyImmediate(gridVisualizer.transform.GetChild(i).gameObject);
            }
        }
    }
}
