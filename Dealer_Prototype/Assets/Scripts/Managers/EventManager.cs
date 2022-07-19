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

    public delegate void GameModeChanged(Enumerations.GameMode gameMode);
    public delegate void GameplayStateChanged(Enumerations.GamePlayState gamePlayState);
    public delegate void GameStateChanged(GameState gameState);
    public delegate void SceneLoaded(string sceneName);
    public delegate void SceneUnloaded(string sceneName);
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
        SceneManager.sceneLoaded += Callback_OnSceneLoaded;
        SceneManager.sceneUnloaded += Callback_OnSceneUnloaded;
    }

    protected override void OnApplicationQuit()
    {
    }

    protected void Callback_OnGameStateChanged(GameState gameState)
    {
        Debug.Log("Game State Changed");
    }

    protected void Callback_OnGameModeChanged(Enumerations.GameMode gameMode)
    {
        Debug.Log("Game Mode Changed");
    }

    protected void Callback_OnGameplayStateChanged(Enumerations.GamePlayState gamePlayState)
    {
        Debug.Log("Gameplay State Changed");
    }

    protected void Callback_OnGameSaved()
    {
        Debug.Log("Game Saved");
    }

    protected void Callback_OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        OnSceneLoaded(scene.name);
    }

    protected void Callback_OnSceneUnloaded(Scene scene)
    {
        OnSceneUnloaded(scene.name);
    }

    protected void Callback_SceneLoaded(string sceneName)
    {
        Debug.Log("Scene loaded: " + sceneName);
    }

    protected void Callback_SceneUnloaded(string sceneName)
    {
        Debug.Log("Scene unloaded: " + sceneName);
    }
}
