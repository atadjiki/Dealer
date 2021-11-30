using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jukebox : Interactable
{

    public override void OnMouseEnter()
    {
        base.OnMouseEnter();
    }

    public override void OnMouseExit()
    {
        base.OnMouseExit();
    }

    public override void OnMouseClicked()
    {
        base.OnMouseClicked();

        Debug.Log("Clicked on " + GetID());
    }
}
