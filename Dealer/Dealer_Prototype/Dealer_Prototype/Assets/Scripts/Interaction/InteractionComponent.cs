using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class InteractionComponent : MonoBehaviour
{

    public delegate void OnMouseEnterEvent();
    public delegate void OnMouseExitEvent();

    public OnMouseEnterEvent MouseEnterEvent;
    public OnMouseExitEvent MouseExitEvent;

    private void OnMouseEnter()
    {
        MouseEnterEvent();
    }

    void OnMouseExit()
    {
        MouseExitEvent();
    }
}
