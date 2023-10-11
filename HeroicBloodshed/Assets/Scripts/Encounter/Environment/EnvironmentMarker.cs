using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentMarker : MonoBehaviour
{
    [SerializeField] private GameObject DebugMesh;

    private bool _occupied = false;

    private void Awake()
    {
        DebugMesh.gameObject.SetActive(false);
    }

    public void SetOccupied(bool flag)
    {
        _occupied = flag;
    }

    public bool IsOccupied()
    {
        return _occupied;
    }
}
