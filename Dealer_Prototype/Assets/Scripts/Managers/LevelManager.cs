using Constants;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : Singleton<LevelManager>
{
    private Dictionary<Enumerations.SceneType, Enumerations.SceneName> _map;

    protected override void Awake()
    {
        base.Awake();

        _map = new Dictionary<Enumerations.SceneType, Enumerations.SceneName>();

        PerformLoad(Enumerations.SceneName.Scene_CameraRig);
    }

    protected override void Start()
    {
        base.Start();

        EventManager.Instance.OnSceneLoaded += OnSceneLoaded;
        EventManager.Instance.OnSceneUnloaded += OnSceneUnloaded;
    }

    protected void OnSceneLoaded(Enumerations.SceneName SceneName)
    {

    }

    protected void OnSceneUnloaded(Enumerations.SceneName SceneName)
    {

    }

    public bool IsSceneLoaded(Enumerations.SceneName sceneName)
    {
        Scene scene = SceneManager.GetSceneByName(sceneName.ToString());

        if (scene != null)
        {
            return scene.isLoaded;
        }

        return false;
    }

    private bool PerformLoad(Enumerations.SceneName sceneName)
    {
        if(sceneName == Enumerations.SceneName.Null) { return false; }
        if (IsSceneLoaded(sceneName))
        {
            if (debug) Debug.Log("Scene " + sceneName.ToString() + " is already loaded");
            return false;
        }

        StartCoroutine(Coroutine_PerformLoad(sceneName));
        return true;
    }

    private IEnumerator Coroutine_PerformLoad(Enumerations.SceneName sceneName)
    {
        GameStateManager.Instance.Loading_Start();
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(sceneName.ToString(), LoadSceneMode.Additive);
        loadOperation.completed += Callback_PerformLoad;

        yield return null;
    }

    private void Callback_PerformLoad(AsyncOperation asyncOperation)
    {
        GameStateManager.Instance.Loading_End();
    }

    private void PerformUnload(Enumerations.SceneName sceneName)
    {
        if (IsSceneLoaded(sceneName))
        {
            StartCoroutine(Coroutine_PerformUnload(sceneName));
        }
    }

    private IEnumerator Coroutine_PerformUnload(Enumerations.SceneName sceneName)
    {
        AsyncOperation unloadOperation = SceneManager.UnloadSceneAsync(sceneName.ToString());
        unloadOperation.completed += Callback_PerformUnload;

        yield return null;
    }

    private void Callback_PerformUnload(AsyncOperation asyncOperation)
    {

    }


    public bool RegisterScene(Enumerations.SceneType type, Enumerations.SceneName name)
    {
        Enumerations.SceneName value;
        if (_map.TryGetValue(type, out value) == false)
        {
            _map.Add(type, name);

            if (PerformLoad(name))
            {
                return true;
            }
        }

        if (debug) Debug.Log("A scene is already registered for " + type);
        return false;
    }

    public bool UnRegisterScene(Enumerations.SceneType type)
    {
        Enumerations.SceneName value;
        if (_map.TryGetValue(type, out value))
        {
            PerformUnload(_map[type]);
            _map.Remove(type);
            return true;
        }

        return false;
    }

    public Enumerations.SceneName HasSceneRegistered(Enumerations.SceneType type)
    {
        if (_map.ContainsKey(type))
        {
            return _map[type];
        }

        return Enumerations.SceneName.Null;
    }
}
