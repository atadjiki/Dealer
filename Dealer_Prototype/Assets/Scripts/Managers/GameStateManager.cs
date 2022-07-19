using Constants;
using UnityEngine;

public class GameStateManager : Singleton<GameStateManager>
{
    private GameState _gameState;

    protected override void Awake()
    {
        base.Awake();

        _gameState = new GameState();
        _gameState.EnqueueGameMode(Enumerations.GameMode.Root);

        EventManager.Instance.OnSceneLoaded += Callback_OnSceneLoaded;

    }

    //callbacks
    private void Callback_OnSceneLoaded(string sceneName)
    {
        if(sceneName == SceneName.Environment_Safehouse)
        {
            ToggleGameMode(Enumerations.GameMode.GamePlay, true);
        }
    }

    private void ToggleGameplayState(Enumerations.GamePlayState state)
    {
        Enumerations.GamePlayState previousState = _gameState.GetGameplayState();

        if(previousState != state)
        {
            _gameState.SetGameplayState(state);

            UpdateGameplayState(previousState, _gameState.GetGameplayState());
        }
        else
        {
           // if (debug) Debug.Log("ToggleGameplayState - Could not transition from " + previousState + " to " + state);
        }
    }

    private void ToggleGameMode(Enumerations.GameMode mode, bool enqueue)
    {
        Enumerations.GameMode previousMode = _gameState.GetActiveMode();

        bool success;
        if (enqueue)
        {
            success = _gameState.EnqueueGameMode(mode);
        }
        else
        {
            success = _gameState.DequeueGameMode(mode);
        }

        if (success)
        {
            UpdateGameMode(previousMode, _gameState.GetActiveMode());
        }

      //  if(debug) Debug.Log("ToggleGameMode - Could not transition from " + previousMode + " to " + mode);
    }

    private void UpdateGameMode(Enumerations.GameMode previousMode, Enumerations.GameMode currentMode)
    {
        EventManager.Instance.OnGameModeChanged(previousMode, currentMode);
    }

    private void UpdateGameplayState(Enumerations.GamePlayState previousState, Enumerations.GamePlayState currentState)
    {
        EventManager.Instance.OnGameplayStateChanged(previousState, currentState);
    }

    private void UpdateGameState()
    {
        EventManager.Instance.OnGameStateChanged(_gameState);
    }

    //events
    public void ToSafehouse()
    {
        ToGameplay();
        ToggleGameplayState(Enumerations.GamePlayState.Safehouse);
    }

    public void Loading_Start()
    {
        ToggleGameMode(Enumerations.GameMode.Loading, true);
    }

    public void Loading_End()
    {
        ToggleGameMode(Enumerations.GameMode.Loading, false);
    }

    public void TogglePause()
    {
        if (_gameState.GetActiveMode() == Enumerations.GameMode.Paused)
        {
            ToggleGameMode(Enumerations.GameMode.Paused, false);
        }
        else
        {
            ToggleGameMode(Enumerations.GameMode.Paused, true);
        }
    }

    public void ToGameplay()
    {
        ToggleGameMode(Enumerations.GameMode.GamePlay, true);
    }

    public void AdjustPlayerMoney(int amount)
    {
        _gameState.GetPlayerData().Money += amount;

        if (debug) Debug.Log("Player Money " + _gameState.GetPlayerData().Money);

        UpdateGameState();
    }

    //state
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


    //helpers
    public Enumerations.GameMode GetGameMode()
    {
        return _gameState.GetActiveMode();
    }

    public Enumerations.GamePlayState GetGameplayState()
    {
        return _gameState.GetGameplayState();
    }

    public GameState GetGameState() { return _gameState; }
}

