using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static Constants;

[RequireComponent(typeof(BoxCollider))]
public class EnvironmentTile : MonoBehaviour
{
    private Vector2 _coordinates;
    private BoxCollider _collider;

    private void Awake()
    {
        _collider = GetComponent<BoxCollider>();

        Ray ray = new Ray(this.transform.position, this.transform.up);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 2.0f))
        {
            if (hit.collider != null)
            {
                GameObject hitObject = hit.collider.gameObject;

                Debug.Log("hit!");

                if (hitObject.GetComponent<EnvironmentObstacle>())
                {
                    EnvironmentObstacle obstacle = hitObject.GetComponent<EnvironmentObstacle>();

                    Debug.Log("Tile intersects with obstacle " + obstacle.name);
                }
            }
        }
    }

    public void Setup(int Row, int Column)
    {
        _coordinates = new Vector2(Row, Column);

        // Debug.Log("Tile created at (" + Row + ", " + Column + ")");
    }

    public Vector2 GetCoordinates()
    {
        return _coordinates;
    }

    private void OnMouseOver()
    {
      //  Debug.Log("Pointer over tile " + _coordinates.ToString());
    }
}
