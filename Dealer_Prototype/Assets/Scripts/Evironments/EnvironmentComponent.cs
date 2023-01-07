using System.Collections;
using System.Collections.Generic;
using Constants;
using GameDelegates;
using UnityEngine;

public class EnvironmentComponent : MonoBehaviour
{
    [SerializeField] protected bool debug = false;

    [SerializeField] protected PlayerSpawner playerSpawner;

    [SerializeField] protected CameraRig cameraRig;

    private void Awake()
    {
        Global.OnPlayerSpawned += OnPlayerSpanwed;

        UIUtility.RequestFadeFromBlack(1.0f);

        StartCoroutine(Coroutine_EnterActionsStart());
    }

    private void OnDestroy()
    {
        ExitActions();
    }

    protected virtual IEnumerator Coroutine_EnterActionsStart()
    {
        if (debug) Debug.Log("Environment " + this.name + " - enter actions");

        yield return Coroutine_PerformEnterActions();
    }

    protected virtual IEnumerator Coroutine_PerformEnterActions()
    {
        SpawnPlayer();

        yield return Coroutine_EnterActionsCompleted();
    }

    protected virtual IEnumerator Coroutine_EnterActionsCompleted()
    {
        yield return null;
    }

    protected virtual void ExitActions()
    {
        if (debug) Debug.Log("Environment " + this.name + " - exit actions");
    }

    protected virtual void SpawnPlayer()
    {
        if (playerSpawner != null)
        {
            playerSpawner.PerformSpawn();
        }
    }

    protected virtual void OnPlayerSpanwed(PlayerComponent playerComponent)
    {
    }
}

