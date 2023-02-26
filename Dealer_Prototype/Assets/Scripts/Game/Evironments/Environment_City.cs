using System.Collections;
using System.Collections.Generic;
using GameDelegates;
using UnityEngine;

public class Environment_City : EnvironmentComponent
{
    private void Start()
    {
        cutscenePlayer.Setup(OnAllScenesFinished);
        cutscenePlayer.ProcessNext();
    }

    private void OnAllScenesFinished()
    {
        GameState.IncrementDay();
        LevelUtility.GoToLoading(LevelUtility.PlayerLocation.Safehouse);
    }
}

