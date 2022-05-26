using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using Constants;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private LevelDataConstants.LevelName initialLevel;

    private enum State { Busy, None };

    private State _state = State.None;

    private static LevelManager _instance;

    public static LevelManager Instance { get { return _instance; } }

    public delegate void OnLevelLoaded(LevelDataConstants.LevelName levelName);
    public delegate void OnLoadStart();
    public delegate void OnLoadProgress(float progress);
    public delegate void OnLoadEnd();

    public OnLevelLoaded onLevelLoaded;
    public OnLoadStart onLoadStart;
    public OnLoadProgress onLoadProgress;
    public OnLoadEnd onLoadEnd;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }

        Build();
    }

    private void Build()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;

        StartCoroutine(LoadLevel(initialLevel));
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("OnSceneLoaded: " + scene.name);
    }

    void OnSceneUnloaded(Scene scene)
    {
        Debug.Log("OnSceneUnloaded: " + scene.name);
    }

    void OnAsyncLoadComplete(AsyncOperation asyncOperation)
    {
        Debug.Log("OnAsyncLoadComplete: " + asyncOperation.ToString());
        asyncOperation.allowSceneActivation = true;
        _state = State.Busy;
        if(onLoadEnd != null) onLoadEnd();
    }

    public IEnumerator LoadLevel(LevelDataConstants.LevelName levelName)
    {
        yield return new WaitForSeconds(5.0f);

        //is this already loaded?
        int buildIndex = SceneUtility.GetBuildIndexByScenePath(levelName.ToString());

        if (buildIndex == -1)
        {
//            Debug.Log("Build index is not valid: " + levelName.ToString());
            yield break;
        }
        else
        {
            Debug.Log(levelName + "| Build index: " + buildIndex);
        }

        Scene scene = SceneManager.GetSceneByBuildIndex(buildIndex);

        if(scene != null)
        {
           if(scene.isLoaded == true)
            {
                Debug.Log(scene.name + " is already loaded.");
                yield break;
            }
            else
            {
                if (_state == State.None)
                {
                    _state = State.Busy;

                    DebugManager.Instance.Print(DebugManager.Log.LogLevelmanager, "loading level " + levelName);

                    if(onLoadStart != null) onLoadStart();

                    AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(buildIndex);
                    asyncLoad.allowSceneActivation = false;
                    asyncLoad.completed += OnAsyncLoadComplete;

                    // Wait until the asynchronous scene fully loads
                    while (!asyncLoad.isDone)
                    {
                        Debug.Log("Load progress " + asyncLoad.progress);
                        if(onLoadProgress != null) onLoadProgress(asyncLoad.progress);
                        yield return new WaitForFixedUpdate();
                    }
                }
                else
                {
                    DebugManager.Instance.Print(DebugManager.Log.LogLevelmanager, "Level manager is busy");
                }
            }
        }
    }

    public static bool IsManagerLoaded()
    {
        return SceneManager.GetSceneByBuildIndex(0).isLoaded;
    }
}