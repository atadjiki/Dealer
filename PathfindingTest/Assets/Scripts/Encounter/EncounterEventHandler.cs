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
        EncounterModel.OnStateChanged += OnStateChangedCallback;
    }

    protected virtual void Dispose()
    {
        EncounterModel.OnStateChanged -= OnStateChangedCallback;
    }

    protected virtual void OnStateChangedCallback(EncounterState state) { }
}
