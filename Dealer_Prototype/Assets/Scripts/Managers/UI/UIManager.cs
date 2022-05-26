using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public enum UIState { None, Gameplay, Conversation };

    private UIState currentState = UIState.None;

    //

    [SerializeField] private InGamePanel inGamePanel;

    //

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

        LevelManager.Instance.onLoadStart += OnLoadStart;
        LevelManager.Instance.onLoadProgress += OnLoadProgress;
        LevelManager.Instance.onLoadEnd += OnLoadEnd;
    }

    private void OnLoadStart()
    {
        //hide in game UI
        inGamePanel.gameObject.SetActive(false);
    }

    private void OnLoadProgress(float progress)
    {
        
    }

    private void OnLoadEnd()
    {
        //bring back in game UI
        if (inGamePanel != null) inGamePanel.gameObject.SetActive(true);
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
