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

    private List<GameObject> ActiveUI;


    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        ActiveUI = new List<GameObject>();

        EventManager.Instance.OnGameModeChanged += OnGameModeChanged;

        AddUIFromGameMode(GameStateManager.Instance.GetGameMode());
    }

    protected override void OnApplicationQuit()
    {
    }

    private void OnGameModeChanged(Enumerations.GameMode gameMode)
    {
        Debug.Log(this.name + " - On Game Mode Changed: " + gameMode.ToString());

        Clear();

        AddUIFromGameMode(gameMode);
    }

    private void  AddUIFromGameMode(Enumerations.GameMode mode)
    {
        ActiveUI.Add(GetPrefabFromGameMode(mode));
    }

    private GameObject GetPrefabFromGameMode(Enumerations.GameMode mode)
    {
        if (mode == Enumerations.GameMode.GamePlay)
        {
            return Instantiate(Prefab_Mode_GamePlay, _canvas.transform);
        }
        else if (mode == Enumerations.GameMode.Loading)
        {
            return Instantiate(Prefab_Mode_GamePlay, _canvas.transform);
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
        foreach(GameObject gameObject in ActiveUI)
        {
            Destroy(gameObject);
        }

        ActiveUI.Clear();
    }
}