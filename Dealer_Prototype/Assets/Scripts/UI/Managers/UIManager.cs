using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public enum UIState { None, Gameplay, Conversation };

    private UIState currentState = UIState.None;

    private static UIManager _instance;

    public static UIManager Instance { get { return _instance; } }

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

        Build();
    }

    public void SetState(UIState newState)
    {
        currentState = newState;
    }

    private void Build()
    {
        GameStateManager.Instance.onModeChanged += OnModeChanged;

        OnModeChanged(GameStateManager.Instance.GetMode());
    }

    private void OnModeChanged(GameStateManager.Mode newMode)
    {
        if(newMode == GameStateManager.Mode.GamePlay)
        {
            SetState(UIState.Gameplay);
        }
        else if(newMode == GameStateManager.Mode.Conversation)
        {
            SetState(UIState.Conversation);
        }
        else
        {
            SetState(UIState.None);
        }
    }
}
