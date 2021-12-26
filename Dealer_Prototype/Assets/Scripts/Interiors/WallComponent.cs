using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallComponent : MonoBehaviour, IInterior
{

    [SerializeField] private Mesh Mesh_Low;
    [SerializeField] private Mesh Mesh_High;

    private MeshFilter filter;

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
        ToState(State.Low);
    }

    public void MouseExit()
    {
        ToState(State.High);
    }
}
