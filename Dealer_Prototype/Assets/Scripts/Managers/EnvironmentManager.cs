using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Constants;

public class EnvironmentManager : Singleton<EnvironmentManager>
{
    [Header("GameMode Prefabs")]
    [SerializeField] private GameObject Prefab_Environment_Safehouse;

    private GameObject ActiveEnvironment = null;

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
        ActiveEnvironment = GetPrefabFromGameplayState(gameplayState);
    }

    private GameObject GetPrefabFromGameplayState(Enumerations.GamePlayState gameplayState)
    {
        if (gameplayState == Enumerations.GamePlayState.Safehouse)
        {
            Debug.Log("loading environment " + Prefab_Environment_Safehouse.name);
            return Instantiate(Prefab_Environment_Safehouse, this.transform);
        }

        return null;
    }

    //clear out existing environment
    private void Clear()
    {
        if(ActiveEnvironment != null)
        {
            Destroy(ActiveEnvironment);

            ActiveEnvironment = null;
        }
    }
}
