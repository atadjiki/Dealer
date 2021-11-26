using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class InteractionComponent : MonoBehaviour
{

    public delegate void OnMouseEnterEvent();
    public delegate void OnMouseExitEvent();
    public delegate void OnMouseClickedEvent();

    public OnMouseEnterEvent MouseEnterEvent;
    public OnMouseExitEvent MouseExitEvent;
    public OnMouseClickedEvent MouseClickedEvent;

    private void OnMouseEnter()
    {
        MouseEnterEvent();
    }

    void OnMouseExit()
    {
        MouseExitEvent();
    }

    private void OnMouseDown()
    {
        MouseClickedEvent();
    }
}
