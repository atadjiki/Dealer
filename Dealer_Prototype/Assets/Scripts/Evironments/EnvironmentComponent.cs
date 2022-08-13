using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentComponent : MonoBehaviour
{
    [SerializeField] private bool debug = false;

    private void Awake()
    {
        EnterActions();
    }

    private void OnDestroy()
    {
        ExitActions();
    }

    protected virtual void EnterActions()
    {
        if (debug) Debug.Log("Environment " + this.name + " - enter actions");
    }

    protected virtual void ExitActions()
    {
        if (debug) Debug.Log("Environment " + this.name + " - exit actions");
    }
}
