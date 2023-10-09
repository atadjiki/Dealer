using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EncounterGridSpawnMarker : MonoBehaviour
{
    [SerializeField] private GameObject DebugMesh;

    private bool _occupied = false;

    private void Awake()
    {
        GameObject.Destroy(DebugMesh.gameObject);
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
