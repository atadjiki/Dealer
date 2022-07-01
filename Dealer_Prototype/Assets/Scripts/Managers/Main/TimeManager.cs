using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Constants;

public class TimeManager : Manager
{
    private static TimeManager _instance;

    public static TimeManager Instance { get { return _instance; } }

    private State.TimeMode _currentMode = State.TimeMode.Normal;
    private State.TimeMode _previousMode = State.TimeMode.Normal;

    public State.TimeMode GetTimeMode() { return _currentMode; }

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
        GameStateManager.Instance.onGameModeChanged += OnGameModeChanged;
        GameStateManager.Instance.onGamePlayModeChanged += OnGamePlayModeChanged;
        return base.AssignDelegates();
    }

    public void OnGamePlayModeChanged(State.GamePlayMode GamePlayMode)
    {
        switch(GamePlayMode)
        {
            case State.GamePlayMode.Paused:
                PauseTimeScale();
                break;
            case State.GamePlayMode.Day:
                UnPauseTimeScale();
                break;
            case State.GamePlayMode.Conversation:
                PauseTimeScale();
                break;
            case State.GamePlayMode.PreDay:
                PauseTimeScale();
                break;
            case State.GamePlayMode.PostDay:
                PauseTimeScale();
                break;
            case State.GamePlayMode.RandomEvent:
                PauseTimeScale();
                break;
        }
    }

    public void OnGameModeChanged(State.GameMode GameMode)
    {
        switch(GameMode)
        {
            case State.GameMode.GamePlay:
                UnPauseTimeScale();
                break;
            case State.GameMode.Loading:
                PauseTimeScale();
                break;
            case State.GameMode.MainMenu:
                PauseTimeScale();
                break;
        }
    }

    public void PauseTimeScale()
    {
        _previousMode = _currentMode;
        _currentMode = State.TimeMode.Paused;

        UpdateTimeScale();
    }

    public void UnPauseTimeScale()
    {
        if(_previousMode == State.TimeMode.Paused) { _currentMode = State.TimeMode.Normal; }
        else { _currentMode = _previousMode; }
       
        _previousMode = State.TimeMode.Paused;

        UpdateTimeScale();
    }

    public void UpdateTimeScale()
    {
        Debug.Log("Time Scale: " + Time.timeScale + " (" + _currentMode.ToString() + ")");
    }

    public override bool PerformUpdate(float tick)
    {
        if(_currentMode == State.TimeMode.Paused) { return false; }

        return base.PerformUpdate(tick);
    }
}
