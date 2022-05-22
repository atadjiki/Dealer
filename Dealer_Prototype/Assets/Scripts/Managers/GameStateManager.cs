using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour
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

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    private void Start()
    {
        onStateChanged(state);
    }

    public void ToMode(Mode mode)
    {
        _currentMode = mode;

        onModeChanged(_currentMode);
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
