using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

public class EncounterEventHandler : MonoBehaviour
{
    private void Awake()
    {
        OnAwake();
    }

    private void OnDestroy()
    {
        Dispose();
    }

    protected virtual void OnAwake()
    {
        EncounterStateData.OnStateChanged += OnStateChangedCallback;
    }

    protected virtual void Dispose()
    {
        EncounterStateData.OnStateChanged -= OnStateChangedCallback;
    }

    protected virtual void OnStateChangedCallback(EncounterStateData state) { }
}
