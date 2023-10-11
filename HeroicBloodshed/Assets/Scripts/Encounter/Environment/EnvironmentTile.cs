using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

public class EnvironmentTile : MonoBehaviour
{
    [SerializeField] private EnvironmentTileType _type;

    private Vector2 _coordinates;

    public void Setup(int Row, int Column)
    {
        _coordinates = new Vector2(Row, Column);
    }

    public Vector2 GetCoordinates()
    {
        return _coordinates;
    }

    public void SetType(EnvironmentTileType TileType)
    {
        _type = TileType;
    }
}
