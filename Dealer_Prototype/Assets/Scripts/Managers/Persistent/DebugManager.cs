using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using TMPro;
using UnityEngine;

public class DebugManager : Manager
{
    public enum State { None, LogOnly, VisualLogOnly, LogAndVisual };

    public enum Log
    {
        LogCharacter,
        LogInput,
        LogAStar,
        LogNavigator,
        LogNPCManager,
        LogPlayableManager,
        LogInteractableManager,
        LogDatabase,
        LogSpawner,
        LogCameraManager,
        LogBehavior,
        LogLevelmanager
            
    }

    public State State_Character = State.None;
    public State State_Input = State.None;
    public State State_AStar = State.None;
    public State State_Navigator = State.None;
    public State State_NPCManager = State.None;
    public State State_PlayableManager = State.None;
    public State State_InteractableManager = State.None;
    public State State_Database = State.None;
    public State State_Spawner = State.None;
    public State State_CameraManager = State.None;
    public State State_Behavior = State.None;

    private State GetStateByLog(Log log)
    {
        switch(log)
        {
            case Log.LogAStar:
                return State_AStar;
            case Log.LogBehavior:
                return State_Behavior;
            case Log.LogCameraManager:
                return State_CameraManager;
            case Log.LogCharacter:
                return State_Character;
            case Log.LogDatabase:
                return State_Database;
            case Log.LogInput:
                return State_Input;
            case Log.LogInteractableManager:
                return State_InteractableManager;
            case Log.LogNavigator:
                return State_Navigator;
            case Log.LogNPCManager:
                return State_NPCManager;
            case Log.LogPlayableManager:
                return State_PlayableManager;
            case Log.LogSpawner:
                return State_Spawner;
            default:
            return State.None;
        }
    }

    ///
    public void Print(Log log, string text)
    {
        if(GetStateByLog(log) == State.LogAndVisual || GetStateByLog(log) == State.LogOnly)
        {
            Debug.Log(text);
        }
    }

    private static DebugManager _instance;

    public static DebugManager Instance { get { return _instance; } }

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
}
