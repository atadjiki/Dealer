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

        if(initialLevel != LevelDataConstants.LevelName.None)
        {
            LoadLevel(initialLevel);
        }
    }

    // called second
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("OnSceneLoaded: " + scene.name);
        Debug.Log(mode);
    }

    void OnSceneUnloaded(Scene scene)
    {
        Debug.Log("OnSceneUnloaded: " + scene.name);
    }

    public void LoadLevel(LevelDataConstants.LevelName levelName)
    {
        if(_state == State.None)
        {
            DebugManager.Instance.Print(DebugManager.Log.LogLevelmanager, "loading level " + levelName);
            StartCoroutine(DoLoadLevel(levelName));
        }
        else
        {
            DebugManager.Instance.Print(DebugManager.Log.LogLevelmanager, "Level manager is busy");
        }

    }

    private IEnumerator DoLoadLevel(LevelDataConstants.LevelName levelName)
    {
        SceneManager.LoadSceneAsync(levelName.ToString(), LoadSceneMode.Additive);
        yield return null;
    }

  

}
