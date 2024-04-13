using System;
using System.Collections;
using UnityEngine;
using static Constants;

public class EnvironmentScanner : MonoBehaviour
{
    [Header("Settings")]
    public int ScanRange = 8; //in multiples of unit size

    [Header("Prefabs")]
    [SerializeField] private GameObject Prefab_Debug_Node;

    private void Awake()
    {
        PerformScan();
    }

    private void PerformScan()
    {
        StartCoroutine(Coroutine_PerformScan());

        //first check which tiles are walkable

        //for each walkable tile, scan in 4/6 directions to see the status of their neighbors
    }

    private IEnumerator Coroutine_PerformScan()
    {
        for (int rows = 0; rows < ScanRange; rows++)
        {
            for (int columns = 0; columns < ScanRange; columns++)
            {
                GameObject nodeObject = Instantiate<GameObject>(Prefab_Debug_Node, this.transform);
                EnvironmentDebugNode debugNode = nodeObject.GetComponent<EnvironmentDebugNode>();

                Vector3 nodeOrigin = new Vector3(rows * TileSize, 0, columns * TileSize);
                nodeObject.transform.position = nodeOrigin;

                EnvironmentNodeState state = GetNodeState(nodeOrigin);

                debugNode.SetState(state);

            }
        }

        yield return null;
    }

    private EnvironmentNodeState GetNodeState(Vector3 origin)
    {
        Vector3 groundScanOffset = new Vector3(TileSize / 2, 1, TileSize / 2);
        Vector3 obstacleHalfOffset = new Vector3(TileSize / 2, 2, TileSize / 2);
        Vector3 obstacleFullOffset = new Vector3(TileSize / 2, 3, TileSize / 2);

        bool isWalkable = Physics.Raycast(origin + groundScanOffset, Vector3.down, 1.0f, Layer_Ground);

        bool containsObstacleHalf = Physics.Raycast(origin + obstacleHalfOffset, Vector3.down, 1.0f, Layer_Obstacle_Half);

        bool containsObstacleFull = Physics.Raycast(origin + obstacleFullOffset, Vector3.down, 1.0f, Layer_Obstacle_Full);

        if (containsObstacleHalf)
        {
            return EnvironmentNodeState.Obstacle_Half;
        }
        else if(containsObstacleFull)
        {
            return EnvironmentNodeState.Obstacle_Full;
        }
        else if (isWalkable)
        {
            return EnvironmentNodeState.Walkable;
        }
        else
        {
            return EnvironmentNodeState.Unwalkable;
        }
    }
}
