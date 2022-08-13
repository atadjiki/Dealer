using Constants;
using UnityEngine;

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
    }

    private void OnDestroy()
    {
        EventManager.Instance.UnregisterReceiver(this);
    }

    public void HandleEvent(Enumerations.EventID eventID)
    {
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
            success = _gameState.DequeueGameMode();
           
        }

        if (success && previousMode != GetGameMode())
        {
            EventManager.Instance.BroadcastEvent(Enumerations.EventID.GameModeChanged, GetGameMode().ToString());

            if (debug)
            {
                if (debug) Debug.Log("GameMode - " + previousMode + " to " + GetGameMode());
            }
        }
        else if (debug)
        {
            Debug.Log("ToggleGameMode - Could not transition from " + previousMode + " to " + GetGameMode());
        }


    }

    //events
    public void ToSafehouse()
    {
        SetEnvironment(Enumerations.Environment.Safehouse);
    }

    public void TogglePause()
    {
        if (GetGameMode() == Enumerations.GameMode.Paused)
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

        EventManager.Instance.BroadcastEvent(Enumerations.EventID.GameStateChanged, "money - " + _gameState.GetPlayerData().Money);
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

    public GameState GetGameState() { return _gameState; }

    public void SetEnvironment(Enumerations.Environment environment)
    {
        _gameState.SetEnvironment(environment);
        EventManager.Instance.BroadcastEvent(Enumerations.EventID.EnvironmentChanged, environment.ToString());
    }

    public Enumerations.Environment GetEnvironment() { return _gameState.GetEnvironment(); }
}

