using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Constants;

public class GameStateManager : Singleton<GameStateManager>
{
    public Enumerations.GameMode DefaultGameMode;
    public Enumerations.GamePlayState DefaultGameplayState;

    private Enumerations.GameMode _gameMode; public Enumerations.GameMode GetGameMode() { return _gameMode; }
    private Enumerations.GamePlayState _gameplayState; Enumerations.GamePlayState GetGameplayState() { return _gameplayState; }

    protected override void Start()
    {
        PerformLoad();
        UpdateGameMode(DefaultGameMode);
        UpdateGameplayState(DefaultGameplayState);
    }

    protected override void OnApplicationQuit()
    {
        PerformSave();
    }

    private void UpdateGameMode(Enumerations.GameMode newMode)
    {
        _gameMode = newMode;
        EventManager.Instance.OnGameModeChanged(_gameMode);  
    }

    private void UpdateGameplayState(Enumerations.GamePlayState newState)
    {
        _gameplayState = newState;
        EventManager.Instance.OnGameplayStateChanged(_gameplayState);
    }

    public void ToPause()
    {
        UpdateGameMode(Enumerations.GameMode.Paused);
    }

    public void ToGamePlay()
    {
        UpdateGameMode(Enumerations.GameMode.GamePlay);
    }

    protected override void PerformSave()
    {
        base.PerformSave();
    }

    protected override void PerformLoad()
    {
        base.PerformLoad();
    }
}
