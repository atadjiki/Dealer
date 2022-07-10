using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Constants;

public class GameStateManager : Manager
{
    private State.GameMode _gameMode = State.GameMode.Loading;
    private State.GamePlayMode _gamePlayMode = State.GamePlayMode.Paused;

    public State.GameMode GetGameMode() { return _gameMode; }
    public State.GamePlayMode GetGamePlayMode() { return _gamePlayMode; }

    public delegate void OnGameModeChanged(State.GameMode gameMode);
    public OnGameModeChanged onGameModeChanged;

    public delegate void OnGamePlayModeChanged(State.GamePlayMode gamePlayMode);
    public OnGamePlayModeChanged onGamePlayModeChanged;

    public delegate void OnStateChanged(GameState gameState);
    public OnStateChanged onGameStateChanged;

    [SerializeField] public GameState _gameState;

    public State.GameMode InitialGameMode = State.GameMode.GamePlay;

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

        _gameState = GetComponent<GameState>();

        base.Build();
    }

    public override void Activate()
    {
        base.Activate();

        ToGameMode(InitialGameMode);
    }

    public void OnLoadEnd(GameConstants.GameMode levelName)
    {
        if(levelName == GameConstants.GameMode.Apartment)
        {
            ToGameMode(State.GameMode.GamePlay);
            ToGamePlayMode(State.GamePlayMode.PreDay);
            GameStateUpdate();
        }
    }

    public void ToGameMode(State.GameMode gameMode)
    {
        Debug.Log("Game mode changed " + gameMode.ToString());

        _gameMode = gameMode;

        onGameModeChanged(_gameMode);
    }

    public void ToGamePlayMode(State.GamePlayMode gamePlayMode)
    {
        _gamePlayMode = gamePlayMode;

        onGamePlayModeChanged(_gamePlayMode);
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
