using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static Constants;

public class EnvironmentTile : MonoBehaviour
{
    private Vector2 _coordinates;

    public void Setup(int Row, int Column)
    {
        _coordinates = new Vector2(Row, Column);
    }

    public Vector2 GetCoordinates()
    {
        return _coordinates;
    }

    private void OnMouseOver()
    {
        Debug.Log("Pointer over tile " + _coordinates.ToString());
    }
}
