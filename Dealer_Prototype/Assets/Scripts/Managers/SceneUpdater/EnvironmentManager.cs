using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Constants;

public class EnvironmentManager : Singleton<EnvironmentManager>, IEventReceiver
{
    Enumerations.GamePlayState previous = Enumerations.GamePlayState.None;

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
        if(eventID == Enumerations.EventID.GameplayStateChanged)
        {
            HandleGameplayStateChanged();
        }
    }

    private void HandleGameplayStateChanged()
    {
        Enumerations.GamePlayState current = GameStateManager.Instance.GetGameplayState();

        //for safehouse
        if(current == Enumerations.GamePlayState.Safehouse)
        {
            LevelManager.Instance.RegisterScene(Enumerations.SceneType.Environment, SceneName.Environment_Safehouse);
        }
        else if(previous == Enumerations.GamePlayState.Safehouse)
        {
            LevelManager.Instance.UnRegisterScene(Enumerations.SceneType.Environment, SceneName.Environment_Safehouse);
        }

        previous = current;
    }
}
