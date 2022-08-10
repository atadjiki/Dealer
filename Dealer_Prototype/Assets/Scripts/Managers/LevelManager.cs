using Constants;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : Singleton<LevelManager>, IEventReceiver
{
    private Dictionary<string, HashSet<string>> _map;

    protected override void Awake()
    {
        base.Awake();

        InitMap();
    }

    protected override void Start()
    {
        base.Start();

        EventManager.Instance.RegisterReceiver(this);
    }

    private void OnDestroy()
    {
        EventManager.Instance.UnregisterReceiver(this);
    }

    public void HandleEvent(Enumerations.EventID eventID)
    {
        
    }

    public bool RegisterScene(Enumerations.SceneType type, string name)
    {
        if(name == null) { return false; }

        string key = type.ToString();

        if (!_map[key].Contains(name))
        {
            StartCoroutine(Coroutine_PerformLoad(type, name, Enumerations.AllowSceneActivation(type)));
            return true;
        }
        else
        {
            if (debug) Debug.Log("Cant register " + name +  ", it is already registered for " + type);
            return false;
        }
    }

    private IEnumerator Coroutine_PerformLoad(Enumerations.SceneType type, string name, bool allowSceneActivation)
    {
        string key = type.ToString();

        if(Enumerations.RequiresLoadingScreen(type)) GameStateManager.Instance.Loading_Start();

        _map[key].Add(name);

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

        if (Enumerations.RequiresLoadingScreen(type)) GameStateManager.Instance.Loading_End();

        if(debug) DumpSceneMap();

        yield return null;
    }

    public bool UnRegisterScene(Enumerations.SceneType type, string name)
    {
        string key = type.ToString();

        if (_map[key].Count > 0)
        {
            StartCoroutine(Coroutine_PerformUnload(key, name));
            return true;
        }
        else
        {
            if (debug) Debug.Log("A scene has not been registered for " + type + " - " + _map[key]);
            return false;
        }
    }

    private IEnumerator Coroutine_PerformUnload(string key, string name)
    {
        yield return new WaitForSeconds(0.1f);

        SceneManager.UnloadSceneAsync(name);

        _map[key].Remove(name);

        if (debug) DumpSceneMap();

        yield return null;
    }

    public bool HasSceneRegistered(Enumerations.SceneType type, string name)
    {
        string key = type.ToString();

        if (_map.ContainsKey(key))
        {
            return _map[key].Contains(name);
        }

        return false;
    }

    //helpers

    private void DumpSceneMap()
    {
        string output = "Scenes: \n";

        foreach(string key in _map.Keys)
        {
            output += "[ " + key + "] - ";

            foreach (string scene in _map[key])
            {
                output += scene + " , ";
            }

            output += "\n";
        }

        if(debug) Debug.Log(output);
    }

    private void InitMap()
    {
        _map = new Dictionary<string, HashSet<string>>()
        {
            {
                Enumerations.SceneType.Root.ToString(), new HashSet<string>()
            },
            {
                Enumerations.SceneType.Environment.ToString(), new HashSet<string>()
            },
            {
                Enumerations.SceneType.UI.ToString(), new HashSet<string>()
            },
        };
    }
}

