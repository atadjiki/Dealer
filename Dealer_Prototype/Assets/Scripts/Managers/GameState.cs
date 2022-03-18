using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{
    private static GameState _instance;

    public static GameState Instance { get { return _instance; } }

    public enum State { MainMenu, GamePlay, Conversation, GamePlayPaused, Loading };

    private State _currentState = State.MainMenu;

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

    public void ToState(State state)
    {
        _currentState = state;
    }

    public State GetState() { return _currentState; }

}
