using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using Constants;
using UnityEngine;

public class LevelManager : Manager
{
    [SerializeField] private LevelConstants.LevelName initialLevel;

    private enum State { Busy, None };

    private State _state = State.None;

    private static LevelManager _instance;

    public static LevelManager Instance { get { return _instance; } }

    public delegate void OnLoadStart(LevelConstants.LevelName levelName);
    public delegate void OnLoadProgress(LevelConstants.LevelName levelName, float progress);
    public delegate void OnLoadEnd(LevelConstants.LevelName levelName);


    public OnLoadStart onLoadStart;
    public OnLoadProgress onLoadProgress;
    public OnLoadEnd onLoadEnd;

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
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;

        return 2;
    }

    public override void Activate()
    {
        LoadLevel(initialLevel);
        base.Activate();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("OnSceneLoaded: " + scene.name);
    }

    void OnSceneUnloaded(Scene scene)
    {
        Debug.Log("OnSceneUnloaded: " + scene.name);
    }

    void OnAsyncLoadComplete(LevelConstants.LevelName levelName, AsyncOperation asyncOperation)
    {
        Debug.Log("OnAsyncLoadComplete: " + levelName.ToString());
        _state = State.Busy;
        asyncOperation.allowSceneActivation = true;
        onLoadEnd(levelName);
    }

    public void LoadLevel(LevelConstants.LevelName levelName)
    {
        if(IsLevelActive(levelName))
        {
        //    Debug.Log("Cannot load " + levelName + ", as it is already active");
        }
        else
        {
            StartCoroutine(LoadLevel_Coroutine(levelName));
        }   
    }

    private IEnumerator LoadLevel_Coroutine(LevelConstants.LevelName levelName)
    {
        if (_state == State.Busy)
        {
            DebugManager.Instance.Print(DebugManager.Log.LogLevelmanager, "Level manager is busy");
            yield break;
        }

        if (IsSceneLoaded(levelName) > -1 )
        {
            _state = State.Busy;

            DebugManager.Instance.Print(DebugManager.Log.LogLevelmanager, "loading level " + levelName);

            onLoadStart(levelName);

            yield return new WaitForSeconds(0.1f);

            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(levelName.ToString(), LoadSceneMode.Additive);
            asyncLoad.allowSceneActivation = false;

            // Wait until the asynchronous scene fully loads
            while (!asyncLoad.isDone)
            {
                onLoadProgress(levelName, asyncLoad.progress);
                yield return new WaitForSeconds(0.1f);

                if (asyncLoad.progress == 0.9f) 
                { 
                    OnAsyncLoadComplete(levelName, asyncLoad);
                    yield break;
                }
            }
        }
    }

    public static bool IsLevelActive(LevelConstants.LevelName levelName)
    {
        int sceneCount = SceneManager.sceneCount;

        for (int i = 0; i < sceneCount; i++)
        {
            Scene scene = SceneManager.GetSceneAt(i);
            if (scene.name == levelName.ToString())
            {
                return true;
            }
        }

        return false;
    }

    public static int IsSceneLoaded(LevelConstants.LevelName levelName)
    {
        int buildIndex;

        if (IsLevelValid(levelName, out buildIndex))
        {
            Scene scene = SceneManager.GetSceneByBuildIndex(buildIndex);

            if (scene != null)
            {
                if (scene.isLoaded == true)
                {
                    Debug.Log(scene.name + " is loaded.");
                }
            }
        }

        return buildIndex;
    }

    public static bool IsLevelValid(LevelConstants.LevelName levelName, out int buildIndex)
    {
        //is this level valid?
        buildIndex = SceneUtility.GetBuildIndexByScenePath(levelName.ToString());

        if (buildIndex == -1)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}