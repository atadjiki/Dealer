using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Constants;

public class EnvironmentManager : Singleton<EnvironmentManager>, IEventReceiver
{
    Enumerations.Environment previousEnvironment = Enumerations.Environment.None;

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
        if(eventID == Enumerations.EventID.EnvironmentChanged)
        {
            HandleEnvironmentChanged();
        }
    }

    private void HandleEnvironmentChanged()
    {
        Enumerations.Environment currentEnvironment = GameStateManager.Instance.GetEnvironment();

        //for safehouse
        if (currentEnvironment == Enumerations.Environment.Safehouse)
        {
            LevelManager.Instance.RegisterScene(Enumerations.SceneType.Environment, SceneName.Environment_Safehouse);
        }
        else if (previousEnvironment == Enumerations.Environment.Safehouse)
        {
            LevelManager.Instance.UnRegisterScene(Enumerations.SceneType.Environment, SceneName.Environment_Safehouse);
        }
    }
}
