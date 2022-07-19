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

        GameStateManager.Instance.ToSafehouse();
    }

    private void OnGameplayStateChanged(Enumerations.GamePlayState previousState, Enumerations.GamePlayState currentState)
    {
        if(previousState != currentState)
        {
            UpdateEnvironment(currentState);
        }
    }

    private void UpdateEnvironment(Enumerations.GamePlayState gameplayState)
    {
        if(LevelManager.Instance.HasSceneRegistered(Enumerations.SceneType.Environment) != Enumerations.SceneName.Null)
        {
            Clear();
        }

        LevelManager.Instance.RegisterScene(Enumerations.SceneType.Environment, Enumerations.GetSceneNameFromGameplayState(gameplayState));
    }

    private void Clear()
    {
        LevelManager.Instance.UnRegisterScene(Enumerations.SceneType.Environment);
    }
}
