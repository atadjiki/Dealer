using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using Constants;

/*
 * This class acts as the messenger for other singletons, handling things like delegates, etc
 * Other scripts can register to this manager, and will receive events which they can manage on their own end
 * For example, the event manager will receive OnLevelLoaded, then transmit that message to the InputManager
 */

public class EventManager : Singleton<EventManager>
{
    //events we will need
    //game mode changed (gameplay, loading, paused, start menu, cutscene, etc)
    //game state changed
    //level loaded, level unloaded

    public delegate void GameModeChanged(Enumerations.GameMode previousMode, Enumerations.GameMode currentMode);
    public delegate void GameplayStateChanged(Enumerations.GamePlayState previousState, Enumerations.GamePlayState currentState);
    public delegate void GameStateChanged(GameState gameState);
    public delegate void SceneLoaded(Enumerations.SceneName SceneName);
    public delegate void SceneUnloaded(Enumerations.SceneName SceneName);
    public delegate void GameSaved();

    public GameModeChanged OnGameModeChanged;
    public GameplayStateChanged OnGameplayStateChanged;
    public GameStateChanged OnGameStateChanged;
    public GameSaved OnGameSaved;
    public SceneLoaded OnSceneLoaded;
    public SceneUnloaded OnSceneUnloaded;

    protected override void Awake()
    {
        base.Awake();

        OnGameplayStateChanged += Callback_OnGameplayStateChanged;
        OnGameModeChanged += Callback_OnGameModeChanged;
        OnGameStateChanged += Callback_OnGameStateChanged;
        OnGameSaved += Callback_OnGameSaved;
        OnSceneLoaded += Callback_SceneLoaded;
        OnSceneUnloaded += Callback_SceneUnloaded;

    }

    protected override void Start()
    {
        base.Start();

        if (debug) Debug.Log("Event: On Start");

        SceneManager.sceneLoaded += Callback_OnSceneLoaded;
        SceneManager.sceneUnloaded += Callback_OnSceneUnloaded;
    }

    protected override void OnApplicationQuit()
    {
        base.OnApplicationQuit();

        if (debug) Debug.Log("Event: On Application Quit");
    }

    protected void Callback_OnGameStateChanged(GameState gameState)
    {
        if (debug) Debug.Log("Event: Game State Changed");
    }

    protected void Callback_OnGameModeChanged(Enumerations.GameMode previousMode, Enumerations.GameMode currentMode)
    {
        if (debug) Debug.Log("Event: Game Mode Changed " + previousMode + " -> " + currentMode);
    }

    protected void Callback_OnGameplayStateChanged(Enumerations.GamePlayState previousState, Enumerations.GamePlayState currentState)
    {
        if (debug) Debug.Log("Event: Gameplay State Changed " + previousState + " -> " + currentState);
    }

    protected void Callback_OnGameSaved()
    {
        if (debug) Debug.Log("Event: Game Saved");
    }

    protected void Callback_OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        OnSceneLoaded(Enumerations.GetSceneNameFromScene(scene));
    }

    protected void Callback_OnSceneUnloaded(Scene scene)
    {
        OnSceneUnloaded(Enumerations.GetSceneNameFromScene(scene));
    }

    protected void Callback_SceneLoaded(Enumerations.SceneName SceneName)
    {
        if (debug) Debug.Log("Event: Scene loaded: " + SceneName);
    }

    protected void Callback_SceneUnloaded(Enumerations.SceneName SceneName)
    {
        if (debug) Debug.Log("Event: Scene unloaded: " + SceneName);
    }
}
