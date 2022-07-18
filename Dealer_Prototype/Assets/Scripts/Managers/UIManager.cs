using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine.UI;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    [Header("Member Variabes")]
    [SerializeField] private Canvas _canvas;

    [Header("GameMode Prefabs")]
    [SerializeField] private GameObject Prefab_Mode_Paused;
    [SerializeField] private GameObject Prefab_Mode_Loading;
    [SerializeField] private GameObject Prefab_Mode_GamePlay;

    private List<UIPanel> ActiveUI;


    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        ActiveUI = new List<UIPanel>();

        EventManager.Instance.OnGameModeChanged += OnGameModeChanged;
        EventManager.Instance.OnGameStateChanged += OnGameStateChanged;

        AddUIFromGameMode(GameStateManager.Instance.GetGameMode());
    }

    protected override void OnApplicationQuit()
    {
    }

    private void OnGameStateChanged(GameState gameState)
    {
        Debug.Log(this.name + " - On Game Mode Changed");

        foreach (UIPanel uiPanel in ActiveUI)
        {
            uiPanel.PerformUpdate();
        }
    }

    private void OnGameModeChanged(Enumerations.GameMode gameMode)
    {
        Debug.Log(this.name + " - On Game Mode Changed: " + gameMode.ToString());

        Clear();

        AddUIFromGameMode(gameMode);
    }

    private void AddUIFromGameMode(Enumerations.GameMode mode)
    {
        GameObject prefab = GetPrefabFromGameMode(mode);

        UIPanel uiPanel = prefab.GetComponent<UIPanel>();

        if(uiPanel != null)
        {
            ActiveUI.Add(uiPanel);

            uiPanel.PerformUpdate();
        }
    }

    private GameObject GetPrefabFromGameMode(Enumerations.GameMode mode)
    {
        if (mode == Enumerations.GameMode.GamePlay)
        {
            return Instantiate(Prefab_Mode_GamePlay, _canvas.transform);
        }
        else if (mode == Enumerations.GameMode.Loading)
        {
            return Instantiate(Prefab_Mode_Loading, _canvas.transform);
        }
        else if (mode == Enumerations.GameMode.Paused)
        {
            return Instantiate(Prefab_Mode_Paused, _canvas.transform);
        }

        return null;
    }

    //clear out existing UI on screen 
    private void Clear()
    {
        foreach (UIPanel uIPanel in ActiveUI)
        {
            if (uIPanel != null)
            {
                Destroy(uIPanel.gameObject);
            }
        }

        ActiveUI.Clear();
    }
}