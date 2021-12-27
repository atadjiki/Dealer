using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallComponent : MonoBehaviour, IInterior
{

    [SerializeField] private Mesh Mesh_Low;
    [SerializeField] private Mesh Mesh_High;

    private MeshFilter filter;

    public bool Locked = false;

    private enum State { Low, High };

    private void Awake()
    {
        filter = GetComponentInChildren<MeshFilter>();

        ToState(State.High);
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

    public void MouseEnter()
    {
        if(!Locked) ToState(State.Low);

        CursorManager.Instance.ToCancel();
    }

    public void MouseExit()
    {
        if (!Locked) ToState(State.High);
    }
}
