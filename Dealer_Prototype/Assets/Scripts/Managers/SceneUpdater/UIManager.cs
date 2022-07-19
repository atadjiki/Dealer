using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine.UI;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    protected override void Start()
    {
        base.Start();

        EventManager.Instance.OnGameModeChanged += OnGameModeChanged;
    }

    private void OnGameModeChanged(Enumerations.GameMode previousMode, Enumerations.GameMode currentMode)
    {
        UpdateUI(currentMode); 
    }

    private void UpdateUI(Enumerations.GameMode mode)
    {
        Clear();
       
        LevelManager.Instance.RegisterScene(Enumerations.SceneType.UI, Enumerations.GetSceneNameFromGameMode(mode));
    }

    private void Clear()
    {
        LevelManager.Instance.UnRegisterScene(Enumerations.SceneType.UI);
    }
}