using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine.UI;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    protected override void Awake()
    {
        base.Awake();

        EventManager.Instance.OnGameModeChanged += OnGameModeChanged;
    }

    private void OnGameModeChanged(Enumerations.GameMode previous, Enumerations.GameMode current)
    {
        HandleModeCases(previous, current);
    }

    private void HandleModeCases(Enumerations.GameMode previous, Enumerations.GameMode current)
    {
        //cases for loading UI
        if (current == Enumerations.GameMode.Loading)
        {
            LevelManager.Instance.RegisterScene(Enumerations.SceneType.UI, SceneName.UI_Loading);
        }
        else if (previous == Enumerations.GameMode.Loading)
        {
            LevelManager.Instance.UnRegisterScene(Enumerations.SceneType.UI, SceneName.UI_Loading);
        }

        //cases for pause UI
        if (current == Enumerations.GameMode.Paused)
        {
            LevelManager.Instance.RegisterScene(Enumerations.SceneType.UI, SceneName.UI_Pause);
        }
        else if (previous == Enumerations.GameMode.Paused)
        {
            LevelManager.Instance.UnRegisterScene(Enumerations.SceneType.UI, SceneName.UI_Pause);
        }

        //cases for gameplay UI
        if (current == Enumerations.GameMode.GamePlay)
        {
            LevelManager.Instance.RegisterScene(Enumerations.SceneType.UI, SceneName.UI_GamePlay);
        }
        else if (previous == Enumerations.GameMode.GamePlay)
        {
            LevelManager.Instance.UnRegisterScene(Enumerations.SceneType.UI, SceneName.UI_GamePlay);
        }
    }
}
