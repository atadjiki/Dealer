using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine;

public class MarkerGroup : MonoBehaviour
{
    public CharacterMarker[] Markers;

    public int GetSize() { return Markers.Length; }
}
