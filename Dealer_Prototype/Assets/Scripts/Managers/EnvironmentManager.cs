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
        EventManager.Instance.OnGameModeChanged += OnGameModeChanged;

        AddEnvironmentFromGameMode(GameStateManager.Instance.GetGameMode());
    }

    protected override void OnApplicationQuit()
    {
    }

    private void OnGameModeChanged(Enumerations.GameMode gameMode)
    {
        Debug.Log(this.name + " - On Game Mode Changed: " + gameMode.ToString());

        Clear();

        AddEnvironmentFromGameMode(gameMode);
    }

    private void AddEnvironmentFromGameMode(Enumerations.GameMode mode)
    {
        ActiveEnvironment = GetPrefabFromGameMode(mode);
    }

    private GameObject GetPrefabFromGameMode(Enumerations.GameMode mode)
    {
        if (mode == Enumerations.GameMode.GamePlay)
        {
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
