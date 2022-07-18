using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Constants;

public class GameStateManager : Singleton<GameStateManager>
{
    public Enumerations.GameMode DefaultGameMode;
    public Enumerations.GamePlayState DefaultGameplayState;

    private GameState _gameState;

    protected override void Awake()
    {
        base.Awake();

        _gameState = new GameState();
    }

    protected override void Start()
    {
        PerformLoad();

        _gameState.gameMode = DefaultGameMode;
        _gameState.gameplayState = DefaultGameplayState;

        Refresh();
        
    }

    protected override void OnApplicationQuit()
    {
        PerformSave();
    }

    public void Refresh()
    {
        UpdateGameMode();
        UpdateGameplayState();
        UpdateGameState();
    }

    private void UpdateGameMode()
    {
        EventManager.Instance.OnGameModeChanged(_gameState.gameMode);
    }

    private void UpdateGameplayState()
    {
        EventManager.Instance.OnGameplayStateChanged(_gameState.gameplayState);
    }

    private void UpdateGameState()
    {
        EventManager.Instance.OnGameStateChanged(_gameState);
    }

    public void ToPause()
    {
        _gameState.gameMode = Enumerations.GameMode.Paused;
        UpdateGameMode();
    }

    public void ToGamePlay()
    {
        _gameState.gameMode = Enumerations.GameMode.GamePlay;
        UpdateGameMode();
    }

    protected override void PerformSave()
    {
        base.PerformSave();

        _gameState.SaveState();
    }

    protected override void PerformLoad()
    {
        base.PerformLoad();

        _gameState.LoadState();
    }

    public Enumerations.GameMode GetGameMode()
    {
        return _gameState.gameMode;
    }

    public Enumerations.GamePlayState GetGameplayState()
    {
        return _gameState.gameplayState;
    }

    public void AdjustPlayerMoney(int amount)
    {
        _gameState.playerData.Money += amount;

        Debug.Log("Player Money " + _gameState.playerData.Money);

        UpdateGameState();
    }

    public GameState GetGameState() { return _gameState; }
}
