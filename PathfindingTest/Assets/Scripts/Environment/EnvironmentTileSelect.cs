using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

public class EnvironmentTileSelect : MonoBehaviour
{
    private void Update()
    {
        TileGraph graph = EnvironmentUtil.GetEnvironmentGraph();

        if(graph != null && Camera.main != null)
        {
            Vector3 mousePosition = Input.mousePosition;

            Ray ray = Camera.main.ScreenPointToRay(mousePosition);
            RaycastHit hit;

            if(Physics.Raycast(ray, out hit, 100))
            {
                Vector3 nearest;
                if (EnvironmentUtil.GetNearestTile(hit.point, out nearest))
                {
                    Debug.Log("Nearest: " + nearest.ToString());
                }
            }
        }
    }
}
