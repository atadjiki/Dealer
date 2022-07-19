using Constants;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : Singleton<LevelManager>
{
    
    private Dictionary<string, Enumerations.SceneName> _map;

    protected override void Awake()
    {
        base.Awake();

        _map = new Dictionary<string, Enumerations.SceneName>();
    }

    protected override void Start()
    {
        base.Start();

        RegisterScene(Enumerations.SceneType.Root, Enumerations.SceneName.Scene_CameraRig);
    }

    public bool RegisterScene(Enumerations.SceneType type, Enumerations.SceneName name)
    {
        string key = type.ToString();

        if (!_map.ContainsKey(key))
        {
            StartCoroutine(Coroutine_PerformLoad(type, name, AllowSceneActivation(type)));
            return true;
        }
        else
        {
            if (debug) Debug.Log("Cant register " + name +  " , a scene is already registered for " + type + " - " + _map[key]);
            return false;
        }
    }

    private IEnumerator Coroutine_PerformLoad(Enumerations.SceneType type, Enumerations.SceneName name, bool allowSceneActivation)
    {
        string key = type.ToString();

        if (type == Enumerations.SceneType.Environment) GameStateManager.Instance.Loading_Start();

        _map.Add(key, name);

        yield return new WaitForSeconds(0.1f);

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(name.ToString(), LoadSceneMode.Additive);
        asyncOperation.allowSceneActivation = allowSceneActivation;

        while (!asyncOperation.isDone)
        {
            if (asyncOperation.progress >= 0.9f)
            {
                asyncOperation.allowSceneActivation = true;
            }

            yield return null;
        }

        if (type == Enumerations.SceneType.Environment) GameStateManager.Instance.Loading_End();

        if(debug) DumpSceneMap();

        yield return null;
    }

    public bool UnRegisterScene(Enumerations.SceneType type)
    {
        string key = type.ToString();

        if (_map.ContainsKey(key))
        {
            string sceneName = _map[key].ToString(); 

            StartCoroutine(Coroutine_PerformUnload(key, sceneName));
            return true;
        }
        else
        {
            if (debug) Debug.Log("A scene has not been registered for " + type + " - " + _map[key]);
            return false;
        }
    }

    private IEnumerator Coroutine_PerformUnload(string key, string sceneName)
    {
        yield return new WaitForSeconds(0.1f);

        SceneManager.UnloadSceneAsync(sceneName);

        _map.Remove(key);

        if (debug) DumpSceneMap();

        yield return null;
    }

    //helper 
    private bool AllowSceneActivation(Enumerations.SceneType type)
    {
        switch (type)
        {
            case Enumerations.SceneType.Environment:
                return false;
            default:
                return true;
        }
    }

    public Enumerations.SceneName HasSceneRegistered(Enumerations.SceneType type)
    {
        string key = type.ToString();

        if (_map.ContainsKey(key))
        {
            return _map[key];
        }

        return Enumerations.SceneName.Null;
    }

    private void DumpSceneMap()
    {
        string output = "Scene Map: \n";

        foreach(string key in _map.Keys)
        {
            output += "( " + key + " , " + _map[key] + " ), ";
        }

        Debug.Log(output);
    }
}

