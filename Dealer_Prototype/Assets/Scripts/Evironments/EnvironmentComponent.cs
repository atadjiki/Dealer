using System.Collections;
using System.Collections.Generic;
using Constants;
using GameDelegates;
using UnityEngine;

public class EnvironmentComponent : MonoBehaviour
{
    [Header("Environment")]
    [SerializeField] protected bool debug = false;

    [SerializeField] protected PlayerSpawner playerSpawner;

    [SerializeField] protected CameraRig cameraRig;

    [SerializeField] protected Transform walkToLocation;

    [SerializeField] protected AudioSource musicSource;

    [SerializeField] protected CutscenePlayer cutscenePlayer;

    protected PlayerComponent _player;

    private void Awake()
    {
        Global.OnPlayerSpawned += OnPlayerSpawned;

        UIUtility.RequestFadeFromBlack(1.0f);

        StartCoroutine(Coroutine_EnterActionsStart());
    }

    private void OnDestroy()
    {
        ExitActions();
    }

    protected virtual void ExitActions()
    {
        if (debug) Debug.Log("Environment " + this.name + " - exit actions");

        Global.OnPlayerSpawned -= OnPlayerSpawned;

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

    protected virtual void SpawnPlayer()
    {
        if (playerSpawner != null)
        {
            playerSpawner.PerformSpawn();
        }
    }

    protected virtual void OnPlayerSpawned(PlayerComponent playerComponent)
    {
        _player = playerComponent;

        StartCoroutine(PerformEntranceScene());
    }

    protected virtual void OnPlayerDestinationReached()
    {
        Global.OnToggleUI(true);
    }

    protected virtual IEnumerator PerformEntranceScene()
    {
        yield return new WaitUntil(() => _player.HasInitialized());

        _player.OnDestinationReached += OnPlayerDestinationReached;

        _player.GoTo(walkToLocation.position);

        yield return new WaitForSeconds(0.5f);

        musicSource.Play();
    }
}

