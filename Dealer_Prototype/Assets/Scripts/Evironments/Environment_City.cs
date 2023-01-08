using System.Collections;
using System.Collections.Generic;
using GameDelegates;
using UnityEngine;

public class Environment_City : EnvironmentComponent
{
    [SerializeField] private CharacterSpawner _characterSpawner;

    protected override void OnPlayerSpawned(PlayerComponent playerComponent)
    {
        base.OnPlayerSpawned(playerComponent);

        _player = playerComponent;

        StartCoroutine(PerformEntranceScene());
    }

    public void OnPlayerDestinationReached()
    {
        Global.OnToggleUI(true);
    }

    private IEnumerator PerformEntranceScene()
    {
        yield return new WaitUntil(() => _player.HasInitialized());

        _player.OnDestinationReached += OnPlayerDestinationReached;

        _player.GoTo(entrace_WalkTo_Location.position);

        _musicSource.Play();
    }

    protected override void ExitActions()
    {
        base.ExitActions();
    }
}
