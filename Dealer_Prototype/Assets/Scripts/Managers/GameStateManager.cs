using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Constants;

public class GameStateManager : Singleton<GameStateManager>
{ 
    private Enumerations.GameMode _gameMode;
    public Enumerations.GameMode GetGameMode() { return _gameMode; }

    protected override void Start()
    {
        PerformLoad();
        UpdateGameMode(_gameMode);
    }

    protected override void OnApplicationQuit()
    {
        PerformSave();
    }

    private void UpdateGameMode(Enumerations.GameMode newMode)
    {
        _gameMode = newMode;
        EventManager.Instance.OnGameModeChanged(_gameMode);  
    }

    public void ToPause()
    {
        UpdateGameMode(Enumerations.GameMode.Paused);
    }

    public void ToGamePlay()
    {
        UpdateGameMode(Enumerations.GameMode.GamePlay);
    }

    protected override void PerformSave()
    {
        base.PerformSave();
        ES3.Save(SaveKeys.GameMode, _gameMode);
    }

    protected override void PerformLoad()
    {
        base.PerformLoad();
        _gameMode = ES3.Load(SaveKeys.GameMode, Enumerations.GameMode.Paused);
    }
}
