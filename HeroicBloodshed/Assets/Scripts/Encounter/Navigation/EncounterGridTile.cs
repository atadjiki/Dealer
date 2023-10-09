using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

public class EncounterGridTile : MonoBehaviour
{
    [SerializeField] private EncounterGridTileType TileType;

    private Vector2 _coordinates;

    private bool _occupied = false;

    public void Setup(int Row, int Column)
    {
        _coordinates = new Vector2(Row, Column);
    }

    public Vector2 GetCoordinates()
    {
        return _coordinates;
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
