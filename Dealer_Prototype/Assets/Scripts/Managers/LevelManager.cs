using Constants;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : Singleton<LevelManager>, IEventReceiver
{
    private Dictionary<string, HashSet<string>> _map;
    private bool bIsLoading = false;

    protected override void Awake()
    {
        base.Awake();

        InitMap();
    }

    protected override void Start()
    {
        base.Start();

        EventManager.Instance.RegisterReceiver(this);

        SceneManager.sceneLoaded += Callback_OnSceneLoaded;
    }

    private void OnDestroy()
    {
        EventManager.Instance.UnregisterReceiver(this);
    }

    public void HandleEvent(Enumerations.EventID eventID)
    {

    }

    protected void Callback_OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(Coroutine_HideLoadingScreen());
    }

    public IEnumerator Coroutine_ShowLoadingScreen()
    {
        bIsLoading = true;
        yield return SceneManager.LoadSceneAsync(SceneName.UI_Loading, LoadSceneMode.Additive);

    }

    public IEnumerator Coroutine_HideLoadingScreen()
    {
        if(bIsLoading)
        {
            bIsLoading = false;
            yield return SceneManager.UnloadSceneAsync(SceneName.UI_Loading);
        }
    }

    public bool RegisterScene(Enumerations.SceneType type, string sceneName)
    {
        if (sceneName == null) return false;

        string key = type.ToString();

        if (!_map[key].Contains(sceneName))
        {
            _map[key].Add(name);

            if (!IsSceneAlreadyLoaded(sceneName))
            {
                StartCoroutine(Coroutine_PerformLoad(type, sceneName, Enumerations.AllowSceneActivation(type)));
                return true;
            }
            else
            {
                return false;
            }

        }
        else
        {
            if (debug) Debug.Log("Cant register " + sceneName + ", it is already registered for " + type);
            return false;
        }
    }

    private IEnumerator Coroutine_PerformLoad(Enumerations.SceneType type, string name, bool allowSceneActivation)
    {
        string key = type.ToString();

        yield return new WaitForSeconds(0.1f);

        yield return Coroutine_ShowLoadingScreen();

        yield return new WaitForSeconds(0.1f);

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(name.ToString(), LoadSceneMode.Additive);
        asyncOperation.allowSceneActivation = allowSceneActivation;

        while (!asyncOperation.isDone)
        {
            if (asyncOperation.progress >= 0.9f)
            {
                yield return new WaitForSeconds(0.1f);

                asyncOperation.allowSceneActivation = true;
            }

            yield return null;
        }

        yield return new WaitForSeconds(0.1f);

        if (debug) DumpSceneMap();

        yield return null;
    }

    public void UnregisterAll(Enumerations.SceneType type)
    {
        string key = type.ToString();

        foreach (string scene in _map[key])
        {
            UnRegisterScene(type, scene);
        }
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

        _map[key].Remove(name);

        if (debug) DumpSceneMap();

        yield return SceneManager.UnloadSceneAsync(name);
    }

    public bool IsSceneAlreadyLoaded(string sceneName)
    {
        Scene scene = SceneManager.GetSceneByName(sceneName);

        if (scene == null) return false;

        return scene.isLoaded;
    }

    //helpers

    private void DumpSceneMap()
    {
        string output = "Scenes: \n";

        foreach (string key in _map.Keys)
        {
            output += "[ " + key + "] - ";

            foreach (string scene in _map[key])
            {
                output += scene + " , ";
            }

            output += "\n";
        }

        if (debug) Debug.Log(output);
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
