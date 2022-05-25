using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using Constants;
using TMPro;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private enum State { Busy, None };

    private State _state = State.None;

    /// ////
    ///

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
    }

    public void LoadLevel(LevelData levelData)
    {
        if(_state == State.None)
        {
            DebugManager.Instance.Print(DebugManager.Log.LogLevelmanager, "loading level " + levelData.Name);
            StartCoroutine(DoLoadLevel(levelData));
        }
        else
        {
            DebugManager.Instance.Print(DebugManager.Log.LogLevelmanager, "Level manager is busy");
        }

    }

    private IEnumerator DoLoadLevel(LevelData levelData)
    {
        yield return null;
    }
    
}
