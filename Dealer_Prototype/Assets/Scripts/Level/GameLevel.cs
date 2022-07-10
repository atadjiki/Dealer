using Constants;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLevel : MonoBehaviour
{
    public State.GameMode ActiveOnMode;
    public GameObject content;

    private void Awake()
    {
        GameStateManager.Instance.onGameModeChanged += OnGameModeChanged;

        GameModeUpdate(GameStateManager.Instance.GetGameMode());
    }
    
    private void GameModeUpdate(State.GameMode gameMode)
    {
        if (gameMode == ActiveOnMode)
        {
            Debug.Log("activate " + this.gameObject.name);
            content.SetActive(true);
        }
        else
        {
            content.gameObject.SetActive(false);
        }
    }

    private void OnGameModeChanged(State.GameMode gameMode)
    {
        GameModeUpdate(gameMode);
    }
}
