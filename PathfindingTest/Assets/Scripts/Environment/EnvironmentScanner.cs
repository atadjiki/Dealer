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
    [SerializeField] private GameObject Prefab_Debug_LineRenderer;

    private void Awake()
    {
        PerformScan();
    }

    private void PerformScan()
    {
        StartCoroutine(Coroutine_PerformScan());
    }

    private IEnumerator Coroutine_PerformScan()
    {
        for (int rows = 0; rows < ScanRange; rows++)
        {
            for (int columns = 0; columns < ScanRange; columns++)
            {
                //first check which tiles are walkable

                GameObject nodeObject = Instantiate<GameObject>(Prefab_Debug_Node, this.transform);
                EnvironmentDebugNode debugNode = nodeObject.GetComponent<EnvironmentDebugNode>();

                Vector3 nodeOrigin = new Vector3(rows * TileSize, 0, columns * TileSize);
                nodeObject.transform.position = nodeOrigin;

                EnvironmentNodeState state = GetNodeState(nodeOrigin);

                debugNode.SetState(state);

                //for each walkable tile, scan in 4/6 directions to see the status of their neighbors

                PerformNeighborCheck(nodeObject.transform, Vector3.forward);
                PerformNeighborCheck(nodeObject.transform, Vector3.back);
                PerformNeighborCheck(nodeObject.transform, Vector3.right);
                PerformNeighborCheck(nodeObject.transform, Vector3.left);
            }
        }

        yield return null;
    }

    private void PerformNeighborCheck(Transform nodeTransform, Vector3 direction)
    {
        Vector3 origin = nodeTransform.position + new Vector3(TileSize / 2, 0.1f, TileSize / 2); ;

        bool obstacleHalfHit = Physics.Raycast(origin, direction, TileSize, Layer_Obstacle_Half);
        bool obstacleFullHit = Physics.Raycast(origin, direction, TileSize, Layer_Obstacle_Full);

        CreateLineRenderer(nodeTransform, direction, obstacleFullHit || obstacleHalfHit);
    }

    private void CreateLineRenderer(Transform parent, Vector3 direction, bool hitObstacle)
    {
        GameObject lineObject = Instantiate<GameObject>(Prefab_Debug_LineRenderer, parent);
        lineObject.transform.position = new Vector3(TileSize / 2, 0.1f, TileSize / 2);

        LineRenderer lineRenderer = lineObject.GetComponent<LineRenderer>();

        Vector3 lineEndPosition = parent.position + (direction * TileSize/2);

        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, parent.position);
        lineRenderer.SetPosition(1, lineEndPosition);

        if(!hitObstacle)
        {
            lineRenderer.colorGradient.colorKeys[0].color = Color.green;
            lineRenderer.material.color = Color.green;
        }
        else
        {
            lineRenderer.colorGradient.colorKeys[0].color = Color.red;
            lineRenderer.material.color = Color.red;
        }
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
