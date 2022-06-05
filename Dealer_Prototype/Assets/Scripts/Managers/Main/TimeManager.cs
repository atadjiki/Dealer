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
    private float dayProgress = 0;

    private float dayLength = 6000;
    [SerializeField] private float dayScale = 3;
    [SerializeField] private float Scale_Slow = 0.5f;
    [SerializeField] private float Scale_Normal = 1f;
    [SerializeField] private float Scale_Fast = 5.0f;
    [SerializeField] private float Scale_VeryFast = 10f;

    public State.TimeMode GetTimeMode() { return _currentMode; }

    public float GetDayProgressAsPercentage() { return dayProgress / (dayLength * dayScale); }

    public string GetDayProgressAsTime()
    {
        float percentage = GetDayProgressAsPercentage();

        float daySeconds = 86400;

        TimeSpan t = TimeSpan.FromSeconds(percentage * daySeconds);

        DateTime time = DateTime.Today.Add(t);
        return time.ToString("hh:mm tt"); // It will give "03:00 AM"
    }

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

    public void CycleTimeScale()
    {
        _previousMode = _currentMode;

        if(_currentMode == State.TimeMode.VeryFast)
        {
            _currentMode = State.TimeMode.Slow;
        }
        else if(_currentMode == State.TimeMode.Fast)
        {
            _currentMode = State.TimeMode.VeryFast;
        }
        else if(_currentMode == State.TimeMode.Normal)
        {
            _currentMode = State.TimeMode.Fast;
        }
        else if(_currentMode == State.TimeMode.Slow)
        {
            _currentMode = State.TimeMode.Normal;
        }

        UpdateTimeScale();
    }

    public float GetScaledTime()
    {
        return Time.deltaTime * Time.timeScale;
    }

    public void UpdateTimeScale()
    {
        switch (_currentMode)
        {
            case State.TimeMode.Paused:
                Time.timeScale = 0.0f;
                break;
            case State.TimeMode.Normal:
                Time.timeScale = Scale_Normal;
                break;
            case State.TimeMode.Slow:
                Time.timeScale = Scale_Slow;
                break;
            case State.TimeMode.Fast:
                Time.timeScale = Scale_Fast;
                break;
            case State.TimeMode.VeryFast:
                Time.timeScale = Scale_VeryFast;
                break;
        }

        Debug.Log("Time Scale: " + Time.timeScale + " (" + _currentMode.ToString() + ")");
    }

    public override bool PerformUpdate(float tick)
    {
        if(_currentMode == State.TimeMode.Paused) { return false; }

        if(GameStateManager.Instance.GetGameMode() == State.GameMode.GamePlay
            && GameStateManager.Instance.GetGamePlayMode() == State.GamePlayMode.Day)
        {
            dayProgress += tick * Time.timeScale;

           if(GetDayProgressAsPercentage() >= 1.0f)
            {
                GameStateManager.Instance.ToGamePlayMode(State.GamePlayMode.Paused);
            }
        }

        return base.PerformUpdate(tick);
    }
}
