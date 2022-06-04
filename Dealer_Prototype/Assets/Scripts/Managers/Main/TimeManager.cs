using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : Manager
{
    private static TimeManager _instance;

    public static TimeManager Instance { get { return _instance; } }

    public enum TimeMode { Slow, Paused, Normal, Fast, VeryFast };

    private TimeMode _currentMode = TimeMode.Normal;
    private TimeMode _previousMode = TimeMode.Normal;
    private float dayProgress = 0;

    private float dayLength = 6000;
    [SerializeField] private float dayScale = 3;
    [SerializeField] private float Scale_Slow = 0.5f;
    [SerializeField] private float Scale_Normal = 1f;
    [SerializeField] private float Scale_Fast = 5.0f;
    [SerializeField] private float Scale_VeryFast = 10f;

    public TimeMode GetTimeMode() { return _currentMode; }

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
        GameStateManager.Instance.onModeChanged += OnGameModeChanged;
        return base.AssignDelegates();
    }

    public void OnGameModeChanged(GameStateManager.Mode GameMode)
    {
        switch(GameMode)
        {
            case GameStateManager.Mode.GamePlayPaused:
                PauseTimeScale();
                break;
            case GameStateManager.Mode.GamePlay:
                UnPauseTimeScale();
                break;
            case GameStateManager.Mode.Conversation:
                PauseTimeScale();
                break;
            case GameStateManager.Mode.Loading:
                PauseTimeScale();
                break;
            case GameStateManager.Mode.MainMenu:
                PauseTimeScale();
                break;
        }
    }

    public void PauseTimeScale()
    {
        _previousMode = _currentMode;
        _currentMode = TimeMode.Paused;

        UpdateTimeScale();
    }

    public void UnPauseTimeScale()
    {
        _currentMode = _previousMode;
        _previousMode = TimeMode.Paused;

        UpdateTimeScale();
    }

    public void CycleTimeScale()
    {
        _previousMode = _currentMode;

        if(_currentMode == TimeMode.VeryFast)
        {
            _currentMode = TimeMode.Slow;
        }
        else if(_currentMode == TimeMode.Fast)
        {
            _currentMode = TimeMode.VeryFast;
        }
        else if(_currentMode == TimeMode.Normal)
        {
            _currentMode = TimeMode.Fast;
        }
        else if(_currentMode == TimeMode.Slow)
        {
            _currentMode = TimeMode.Normal;
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
            case TimeMode.Paused:
                Time.timeScale = 0.0f;
                break;
            case TimeMode.Normal:
                Time.timeScale = Scale_Normal;
                break;
            case TimeMode.Slow:
                Time.timeScale = Scale_Slow;
                break;
            case TimeMode.Fast:
                Time.timeScale = Scale_Fast;
                break;
            case TimeMode.VeryFast:
                Time.timeScale = Scale_VeryFast;
                break;
        }

        Debug.Log("Time Scale: " + Time.timeScale + " (" + _currentMode.ToString() + ")");
    }

    public override bool PerformUpdate(float tick)
    {
        if(GameStateManager.Instance.GetMode() == GameStateManager.Mode.GamePlay)
        {
            dayProgress += tick * Time.timeScale;

           if(GetDayProgressAsPercentage() >= 1.0f)
            {
                GameStateManager.Instance.ToMode(GameStateManager.Mode.GamePlayPaused);
            }
        }

        return base.PerformUpdate(tick);
    }
}
