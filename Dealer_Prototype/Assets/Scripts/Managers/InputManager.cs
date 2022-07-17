using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Constants;

public class InputManager : Singleton<InputManager>
{
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        EventManager.Instance.OnGameModeChanged += OnGameModeChanged;
    }

    protected override void OnApplicationQuit(){}

    public void OnGameModeChanged(Enumerations.GameMode gameMode)
    {
        Debug.Log(this.name + " - On Game Mode Changed: " + gameMode.ToString());
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            if(GameStateManager.Instance.GetGameMode() == Enumerations.GameMode.GamePlay)
            {
                GameStateManager.Instance.ToPause();
            }
            else
            {
                GameStateManager.Instance.ToGamePlay();
            }
        }
    }
}
