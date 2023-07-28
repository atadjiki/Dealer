using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

public class CharacterMarker : MonoBehaviour
{
    private bool _occupied = false;

    public void SetOccupied(bool flag)
    {
        _occupied = flag;
    }

    public bool IsOccupied()
    {
        return _occupied;
    }
}
