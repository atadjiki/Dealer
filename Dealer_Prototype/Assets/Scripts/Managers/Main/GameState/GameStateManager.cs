using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Constants;

public class GameStateManager : Manager
{
    private State.GameMode _gameMode = State.GameMode.GamePlay;
    private State.GamePlayMode _gamePlayMode = State.GamePlayMode.Day;

    public State.GameMode GetGameMode() { return _gameMode; }
    public State.GamePlayMode GetGamePlayMode() { return _gamePlayMode; }

    public delegate void OnGameModeChanged(State.GameMode gameMode);
    public OnGameModeChanged onGameModeChanged;

    public delegate void OnGamePlayModeChanged(State.GamePlayMode gamePlayMode);
    public OnGamePlayModeChanged onGamePlayModeChanged;

    public delegate void OnStateChanged(GameState gameState);
    public OnStateChanged onGameStateChanged;

    [SerializeField] public GameState _gameState;

    private static GameStateManager _instance;

    public static GameStateManager Instance { get { return _instance; } }

    public override void Build()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }

        base.Build();
    }

    public override int AssignDelegates()
    {
        LevelManager.Instance.onLoadEnd += OnLoadEnd;

        return 1;
    }

    public void OnLoadEnd(LevelConstants.LevelName levelName)
    {
        if(levelName == LevelConstants.LevelName.Apartment)
        {
            onGameStateChanged(_gameState);
            ToGameMode(State.GameMode.GamePlay);
            ToGamePlayMode(State.GamePlayMode.Day);
        }
    }

    public void ToGameMode(State.GameMode gameMode)
    {
        _gameMode = gameMode;

        onGameModeChanged(_gameMode);
        onGamePlayModeChanged(_gamePlayMode);
        onGameStateChanged(_gameState);
    }

    public void ToGamePlayMode(State.GamePlayMode gamePlayMode)
    {
        _gamePlayMode = gamePlayMode;

        onGamePlayModeChanged(_gamePlayMode);
        onGameStateChanged(_gameState);
    }

    [InspectorButton("StateUpdate")]
    public bool UpdateGameState;

    private void GameStateUpdate()
    {
        if(onGameStateChanged != null)
        {
            onGameStateChanged(_gameState);
            Debug.Log(_gameState.toString());
        }

    }
}
