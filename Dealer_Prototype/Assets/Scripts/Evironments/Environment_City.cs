using System.Collections;
using System.Collections.Generic;
using GameDelegates;
using UnityEngine;

public class Environment_City : EnvironmentComponent
{
    [Header("City")]
    [SerializeField] private CharacterSpawner _characterSpawner;

    [SerializeField] private List<Cutscene> _cutscenes;

    private int _currentScene = 0;

    protected override void OnPlayerDestinationReached()
    {
        base.OnPlayerDestinationReached();

        ProcessCutscenes();
    }

    private void ProcessCutscenes()
    {
        if(_currentScene < _cutscenes.Count)
        {
            Cutscene cutscene = _cutscenes[_currentScene];
            cutscene.Begin(OnCutsceneFinished);
        }
        else
        {
            Debug.Log("finished with all cutscenes for today");
            LevelUtility.GoToLoading(LevelUtility.PlayerLocation.Safehouse);
        }
    }

    private void OnCutsceneFinished()
    {
        _currentScene++;
        ProcessCutscenes();
    }
}

