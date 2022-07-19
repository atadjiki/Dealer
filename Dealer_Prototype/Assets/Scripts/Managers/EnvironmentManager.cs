using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Constants;

public class EnvironmentManager : Singleton<EnvironmentManager>
{
    [Header("GameMode Prefabs")]
    [SerializeField] private Object Scene_Environment_Safehouse;

    private string ActiveSceneName;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        EventManager.Instance.OnGameplayStateChanged += OnGameplayStateChanged;

        GameStateManager.Instance.Refresh();
    }

    protected override void OnApplicationQuit()
    {
    }

    private void OnGameplayStateChanged(Enumerations.GamePlayState gameplayState)
    {
        Debug.Log(this.name + " - On Gameplay State Changed: " + gameplayState.ToString());

        Clear();

        AddEnvironmentFromGameplayState(gameplayState);
    }

    private void AddEnvironmentFromGameplayState(Enumerations.GamePlayState gameplayState)
    {
        ActiveSceneName = GetSceneFromGameplayState(gameplayState);

        SceneManager.LoadSceneAsync(ActiveSceneName, LoadSceneMode.Additive);
    }

    private string GetSceneFromGameplayState(Enumerations.GamePlayState gameplayState)
    {
        if (gameplayState == Enumerations.GamePlayState.Safehouse)
        {
            Debug.Log("loading environment " + Scene_Environment_Safehouse.name);
            return Scene_Environment_Safehouse.name;
        }

        return null;
    }

    //clear out existing environment
    private void Clear()
    {
        if(ActiveSceneName != null)
        {
            SceneManager.UnloadSceneAsync(ActiveSceneName, UnloadSceneOptions.None);

            ActiveSceneName = null;
        }
    }
}
