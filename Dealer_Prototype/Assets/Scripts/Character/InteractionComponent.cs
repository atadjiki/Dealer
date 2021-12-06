using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class InteractionComponent : MonoBehaviour, IInteractionInterface
{

    public delegate void OnMouseEnterEvent();
    public delegate void OnMouseExitEvent();
    public delegate void OnMouseClickedEvent();

    public OnMouseEnterEvent MouseEnterEvent;
    public OnMouseExitEvent MouseExitEvent;
    public OnMouseClickedEvent MouseClickedEvent;

    public virtual void MouseEnter()
    {
        if (MouseEnterEvent != null) MouseEnterEvent();
    }

    public virtual void MouseClick()
    {
        if (MouseClickedEvent != null) MouseClickedEvent();
    }
}
