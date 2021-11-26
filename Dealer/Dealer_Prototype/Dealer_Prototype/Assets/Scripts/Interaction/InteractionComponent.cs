using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class InteractionComponent : MonoBehaviour
{

    private string _InteractionID = "";

    public void SetInteractionID(string _id) { _InteractionID = _id; }

    void OnMouseOver()
    {
        //If your mouse hovers over the GameObject with the script attached, output this message
        Debug.Log("Mouse is over " + _InteractionID);
    }

    void OnMouseExit()
    {
        //The mouse is no longer hovering over the GameObject so output this message each frame
        Debug.Log("Mouse is no longer on " + _InteractionID);
    }
}
