using System.Collections;
using System.Collections.Generic;
using GameDelegates;
using UnityEngine;

public class Environment_City : EnvironmentComponent
{
    [Header("City")]
    [SerializeField] private CharacterSpawner _characterSpawner;

    private void Start()
    {
        cutscenePlayer.Setup(OnAllScenesFinished);
    }

    protected override void OnPlayerDestinationReached()
    {
        base.OnPlayerDestinationReached();

        cutscenePlayer.ProcessNext();
    }

    private void OnAllScenesFinished()
    {
        LevelUtility.GoToLoading(LevelUtility.PlayerLocation.Safehouse);
    }
}

