using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class CameraBounds : MonoBehaviour
{
    private BoxCollider _collider;

    private void Awake()
    {
        _collider = GetComponent<BoxCollider>();

        this.gameObject.layer = 2; //make sure we are ignoring raycasts!
    }
    public bool Contains(Vector3 point)
    {
        return _collider.bounds.Contains(point);
    }

    public Vector3 ClosestPointOnBounds(Vector3 point)
    {
        return _collider.bounds.ClosestPoint(point);
    }
}
