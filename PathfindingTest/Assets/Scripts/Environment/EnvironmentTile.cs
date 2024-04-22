using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

public class EnvironmentTile
{
    private EnvironmentLayer _layer;

    private Vector2 _coordinates;

    private List<EnvironmentTile> _neighbors;

    public EnvironmentTile(int _row, int _column, EnvironmentLayer _layer)
    {
        _coordinates = new Vector2(_row, _column);
        this._layer = _layer;
        _neighbors = new List<EnvironmentTile>();
    }

    public override string ToString()
    {
        return "Tile " + GetOrigin().ToString() + " - " + _layer.ToString();
    }

    public Vector3 GetOrigin()
    {
        return EnvironmentUtil.CalculateTileOrigin((int)_coordinates.x, (int)_coordinates.y);
    }

    public Vector2 GetCoordinates()
    {
        return _coordinates;
    }

    public EnvironmentLayer GetLayer()
    {
        return _layer;
    }

    public List<EnvironmentTile> GetNeighbors()
    {
        return _neighbors;
    }

    public void AddNeighbor(EnvironmentTile neighbor)
    {
        _neighbors.Add(neighbor);
    }
}