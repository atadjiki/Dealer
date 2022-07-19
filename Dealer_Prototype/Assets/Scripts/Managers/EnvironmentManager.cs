using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Constants;

public class EnvironmentManager : Singleton<EnvironmentManager>
{
    [Header("GameMode Prefabs")]
    [SerializeField] private Object Scene_Environment_Safehouse;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        EventManager.Instance.OnGameplayStateChanged += OnGameplayStateChanged;

        GameStateManager.Instance.Refresh();
    }

    protected override void OnApplicationQuit()
    {
    }

    private void OnGameplayStateChanged(Enumerations.GamePlayState gameplayState)
    {
        Debug.Log(this.name + " - On Gameplay State Changed: " + gameplayState.ToString());

        Clear();

        AddEnvironmentFromGameplayState(gameplayState);
    }

    private void AddEnvironmentFromGameplayState(Enumerations.GamePlayState gameplayState)
    {
        LevelManager.Instance.RegisterScene(this, GetSceneFromGameplayState(gameplayState));
    }

    private string GetSceneFromGameplayState(Enumerations.GamePlayState gameplayState)
    {
        if (gameplayState == Enumerations.GamePlayState.Safehouse)
        {
            return Scene_Environment_Safehouse.name;
        }

        return null;
    }

    private void Clear()
    {
        LevelManager.Instance.UnRegisterScene(this);
    }
}

