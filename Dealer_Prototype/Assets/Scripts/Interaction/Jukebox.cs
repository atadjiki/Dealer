using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jukebox : Interactable
{

    public override void MouseEnter()
    {
        base.MouseEnter();
    }

    public override void MouseClick()
    {
        base.MouseClick();

        Debug.Log("Clicked on " + GetID());
    }
}
