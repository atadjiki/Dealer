using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using UnityEditor;

public class EncounterGridBuilder : MonoBehaviour
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
            Vector3 pos = ((Vector3)node.position);

            int col = (index % Columns);
            int row = (index / Columns);

            string tilename = "Tile " + (index+1) + " [ " + row + "," + col + " ] "; ;

            GameObject tileDecal = Instantiate<GameObject>(TilePrefab, pos, Quaternion.identity, this.transform);
            tileDecal.name = tilename;

            Debug.Log(tilename);

            index++;

            return true;
        });
    }
}

[CustomEditor(typeof(EncounterGridBuilder))]
public class EncounterGridBuilderEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EncounterGridBuilder gridVisualizer = (EncounterGridBuilder)target;

        if (GUILayout.Button("Generate Tiles"))
        {

            gridVisualizer.GenerateTiles();
        }

        if(GUILayout.Button("Clear"))
        {
            for (int i = 0; i < gridVisualizer.transform.childCount; i++)
            {
                DestroyImmediate(gridVisualizer.transform.GetChild(i).gameObject);
            }
        }
    }
}
