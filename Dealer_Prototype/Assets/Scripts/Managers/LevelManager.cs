using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : Singleton<LevelManager>
{
    private Dictionary<Object, string> SceneMap;

    protected override void Awake()
    {
        base.Awake();

        SceneMap = new Dictionary<Object, string>();
    }

    protected override void Start()
    {
        EventManager.Instance.OnSceneLoaded += OnSceneLoaded;
        EventManager.Instance.OnSceneUnloaded += OnSceneUnloaded;
    }

    protected override void OnApplicationQuit()
    {
        base.OnApplicationQuit();

        foreach(Object key in SceneMap.Keys)
        {
            SceneManager.UnloadSceneAsync(SceneMap[key]);
        }
    }

    protected void OnSceneLoaded(string sceneName)
    {

    }

    protected void OnSceneUnloaded(string sceneName)
    {

    }

    public void RegisterScene(Object caller, string sceneName)
    {
        if(caller == null)
        {
            Debug.Log("caller is null!");
            return;
        }

        if(!SceneMap.ContainsKey(caller))
        {
            SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            SceneMap.Add(caller, sceneName);
            return;
        }

        Debug.Log("A scene is already registered for object " + caller.name);
    }

    public void UnRegisterScene(Object caller)
    {
        if (caller == null)
        {
            Debug.Log("caller is null!");
            return;
        }

        if (SceneMap.ContainsKey(caller))
        {
            SceneManager.UnloadSceneAsync(SceneMap[caller]);
            SceneMap.Remove(caller);
        }
    }
}

