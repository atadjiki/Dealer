using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine.UI;
using UnityEngine;

public class UIManager : Singleton<UIManager>, IEventReceiver
{
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();

        EventManager.Instance.RegisterReceiver(this);
    }

    private void OnDestroy()
    {
        EventManager.Instance.UnregisterReceiver(this);
    }

    public void HandleEvent(Enumerations.EventID eventID)
    {
        if (eventID == Enumerations.EventID.GameModeChanged)
        {
            HandleModeCases();
        }
    }

    private void HandleModeCases()
    {
        Enumerations.GameMode currentGameMode = GameStateManager.Instance.GetGameMode();

        LevelManager.Instance.UnregisterAll(Enumerations.SceneType.UI);

        switch (currentGameMode)
        {
            case Enumerations.GameMode.GamePlay:
                LevelManager.Instance.RegisterScene(Enumerations.SceneType.UI, SceneName.UI_GamePlay);
                break;
            case Enumerations.GameMode.Paused:
                LevelManager.Instance.RegisterScene(Enumerations.SceneType.UI, SceneName.UI_Pause);
                break;
        }
    }
}