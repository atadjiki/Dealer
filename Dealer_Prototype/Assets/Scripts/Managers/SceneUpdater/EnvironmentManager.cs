using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Constants;

public class EnvironmentManager : Singleton<EnvironmentManager>
{
    protected override void Awake()
    {
        base.Awake();

        EventManager.Instance.OnGameplayStateChanged += OnGameplayStateChanged;
    }

    protected override void Start()
    {
        base.Start();
    }

    private void OnGameplayStateChanged(Enumerations.GamePlayState previous, Enumerations.GamePlayState current)
    {
        HandleGameplayStateChanged(previous, current);
    }

    private void HandleGameplayStateChanged(Enumerations.GamePlayState previous, Enumerations.GamePlayState current)
    {
        //for safehouse
        if(current == Enumerations.GamePlayState.Safehouse)
        {
            LevelManager.Instance.RegisterScene(Enumerations.SceneType.Environment, SceneName.Environment_Safehouse);
        }
        else if(previous == Enumerations.GamePlayState.Safehouse)
        {
            LevelManager.Instance.UnRegisterScene(Enumerations.SceneType.Environment, SceneName.Environment_Safehouse);
        }
    }
}
