using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : Manager
{
    public enum Mode { MainMenu, GamePlay, Conversation, GamePlayPaused, Loading };

    private Mode _currentMode = Mode.MainMenu;

    public delegate void OnModeChanged(Mode state);
    public OnModeChanged onModeChanged;

    public delegate void OnStateChanged(GameState state);
    public OnStateChanged onStateChanged;

    [SerializeField] public GameState state;

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

    public void OnLoadEnd(Constants.LevelConstants.LevelName levelName)
    {
        if(levelName == Constants.LevelConstants.LevelName.Apartment)
        {
            ToMode(Mode.GamePlay);
        }
    }

    public void ToMode(Mode mode)
    {
        _currentMode = mode;

        onModeChanged(_currentMode);
        onStateChanged(state);
    }

    public Mode GetMode() { return _currentMode; }

    [InspectorButton("StateUpdate")]
    public bool UpdateState;

    private void StateUpdate()
    {
        if(onStateChanged != null)
        {
            onStateChanged(state);
            Debug.Log(state.toString());
        }

    }
}
