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
    public delegate void GameStateChanged();
    public delegate void SceneLoaded(string sceneName);
    public delegate void SceneUnloaded(string sceneName);
    public delegate void GameSaved();

    public GameModeChanged OnGameModeChanged;
    public GameStateChanged OnGameStateChanged;
    public SceneLoaded OnSceneLoaded;
    public SceneUnloaded OnSceneUnloaded;
    public GameSaved OnGameSaved;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        SceneManager.sceneLoaded += Callback_SceneLoaded;
        SceneManager.sceneUnloaded += Callback_SceneUnloaded;
    }

    protected override void OnApplicationQuit()
    {
    }

    private void Callback_SceneLoaded(Scene scene, LoadSceneMode mode)
    {
        OnSceneLoaded(scene.name);
    }

    private void Callback_SceneUnloaded(Scene scene)
    {
        OnSceneUnloaded(scene.name);
    }
}
