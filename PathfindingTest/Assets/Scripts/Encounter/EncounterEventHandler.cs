using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

public class EncounterEventHandler : MonoBehaviour
{
    private void Awake()
    {
        Setup();
    }

    private void OnDestroy()
    {
        Dispose();
    }

    protected virtual void Setup()
    {
        EncounterStateMachine.OnStateChanged += OnStateChangedCallback;
    }

    protected virtual void Dispose()
    {
        EncounterStateMachine.OnStateChanged -= OnStateChangedCallback;
    }

    protected virtual void OnStateChangedCallback(EncounterState state) { }
}
