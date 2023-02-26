using System.Collections;
using System.Collections.Generic;
using Constants;
using GameDelegates;
using UnityEngine;

public class EnvironmentComponent : MonoBehaviour
{
    [Header("Environment")]
    [SerializeField] protected bool debug = false;

    [SerializeField] protected CameraRig cameraRig;

    [SerializeField] protected AudioSource musicSource;

    [SerializeField] protected CutscenePlayer cutscenePlayer;

    private void Awake()
    {
        StartCoroutine(Coroutine_EnterActionsStart());
    }

    private void OnDestroy()
    {
        ExitActions();
    }

    protected virtual void ExitActions()
    {
        if (debug) Debug.Log("Environment " + this.name + " - exit actions");

    }

    protected virtual IEnumerator Coroutine_EnterActionsStart()
    {
        if (debug) Debug.Log("Environment " + this.name + " - enter actions");

        yield return Coroutine_PerformEnterActions();
    }

    protected virtual IEnumerator Coroutine_PerformEnterActions()
    {
        yield return Coroutine_EnterActionsCompleted();
    }

    protected virtual IEnumerator Coroutine_EnterActionsCompleted()
    {
        yield return null;
    }
}
