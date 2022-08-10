using Constants;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateManager : Singleton<GameStateManager>, IEventReceiver
{
    private GameState _gameState;

    protected override void Awake()
    {
        base.Awake();

        _gameState = new GameState();
        _gameState.EnqueueGameMode(Enumerations.GameMode.Root);
    }

    protected override void Start()
    {
        base.Start();

        EventManager.Instance.RegisterReceiver(this);

        SceneManager.sceneLoaded += Callback_OnSceneLoaded;
    }

    private void OnDestroy()
    {
        EventManager.Instance.UnregisterReceiver(this);
    }

    protected void Callback_OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(scene != null && scene.name != null)
        {
            HandleEvent_OnSceneLoaded(scene.name);
        }
    }

    private void HandleEvent_OnSceneLoaded(string sceneName)
    {
        //get scene name from somewhere
        if (sceneName == SceneName.Environment_Safehouse)
        {
            ToggleGameMode(Enumerations.GameMode.GamePlay, true);
        }
    }

    public void HandleEvent(Enumerations.EventID eventID)
    {
    }

    private void ToggleGameplayState(Enumerations.GamePlayState state)
    {
        Enumerations.GamePlayState previousState = _gameState.GetGameplayState();

        if(previousState != state)
        {
            _gameState.SetGameplayState(state);

            EventManager.Instance.BroadcastEvent(Enumerations.EventID.GameplayStateChanged);

            if (debug) Debug.Log("GamePlayState - " + previousState + " to " + state);
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
            EventManager.Instance.BroadcastEvent(Enumerations.EventID.GameModeChanged);

            if (debug) Debug.Log("GameMode - " + previousMode + " to " + mode);
        }

        if(debug) Debug.Log("ToggleGameMode - Could not transition from " + previousMode + " to " + mode);
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

        EventManager.Instance.BroadcastEvent(Enumerations.EventID.GameStateChanged);
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

