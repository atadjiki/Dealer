using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EncounterGridVisualizer : MonoBehaviour
{
    [SerializeField] private GameObject TilePrefab;

    private void Awake()
    {
        GridGraph gridGraph = AstarPath.active.data.gridGraph;
        int count = 1;

        gridGraph.GetNodes(node =>
        {
            Debug.Log("Node " + count + " " + node.position.ToString());
  
            Vector3 pos = ((Vector3)node.position);

            GameObject tileDecal = Instantiate<GameObject>(TilePrefab, pos, Quaternion.identity, this.transform);
            tileDecal.name = "Node " + count;

            count++;

            return true;
        });
    }
}
