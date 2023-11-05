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
    }

    public void Setup(int Row, int Column)
    {
        _coordinates = new Vector2(Row, Column);

        foreach (RaycastHit hit in Physics.BoxCastAll(_collider.transform.position, _collider.bounds.extents, _collider.transform.up))
        {
            if (hit.collider != null)
            {
                GameObject hitObject = hit.collider.gameObject;

                if (hitObject.GetComponent<EnvironmentObstacle>())
                {
                    EnvironmentObstacle obstacle = hitObject.GetComponent<EnvironmentObstacle>();

                    Debug.Log("Tile " + _coordinates.ToString() + " intersects with obstacle " + obstacle.name);
                }
            }
        }
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
