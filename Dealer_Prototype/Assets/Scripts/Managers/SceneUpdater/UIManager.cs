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

    private void OnGameModeChanged(Enumerations.GameMode previousMode, Enumerations.GameMode currentMode)
    {

            UpdateUI(currentMode);
        
    }

    private void UpdateUI(Enumerations.GameMode mode)
    {
        string sceneName = SceneName.GetSceneNameFromGameMode(mode);

        if (LevelManager.Instance.HasSceneRegistered(Enumerations.SceneType.UI, sceneName))
        {
            Clear();
        }

        LevelManager.Instance.RegisterScene(Enumerations.SceneType.UI, sceneName);
    }

    private void Clear()
    {
        LevelManager.Instance.UnRegisterScene(Enumerations.SceneType.UI);
    }
}