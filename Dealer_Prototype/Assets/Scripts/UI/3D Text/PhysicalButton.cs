using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicalButton : MonoBehaviour
{
    private void OnMouseDown()
    {
        Debug.Log("Clicked on " + this.name);
    }
}
