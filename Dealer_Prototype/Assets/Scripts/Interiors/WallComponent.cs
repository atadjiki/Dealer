using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine;

public class WallComponent : MonoBehaviour, IInterior
{

    [SerializeField] private Mesh Mesh_Low;
    [SerializeField] private Mesh Mesh_High;

    private MeshFilter filter;

    public bool Locked = false;
    
    public enum State { Low, High };

    public State defaultState = State.High;

    private void Awake()
    {
        filter = GetComponentInChildren<MeshFilter>();

        ToState(defaultState);
    }

    private void ToState(State inState)
    {
        switch(inState)
        {
            case State.High:
                filter.mesh = Mesh_High;
                break;
            case State.Low:
                filter.mesh = Mesh_Low;
                break;
        }
    }

    public void MouseClick(Vector3 location)
    {
    }

    public InteractableConstants.InteractionContext MouseEnter()
    {
        if(!Locked) ToState(State.Low);

        CursorManager.Instance.ToCancel();

        return InteractableConstants.InteractionContext.None;
    }

    public void MouseExit()
    {
        if (!Locked) ToState(State.High);
    }
}
