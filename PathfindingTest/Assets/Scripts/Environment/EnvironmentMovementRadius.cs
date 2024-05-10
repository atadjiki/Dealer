using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

public class EnvironmentMovementRadius : MonoBehaviour
{
    private int distance = 12;
    private List<Vector3> tiles;

    private void Awake()
    {
        tiles = EnvironmentUtil.GetTilesWithinDistance(this.transform.position, distance);
    }

    private void OnDrawGizmosSelected()
    {
        if(Application.isPlaying)
        {
            foreach (Vector3 tile in tiles)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawCube(tile, GetTileSize());
            }
        }

    }
}
