using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Environment_Safehouse : EnvironmentComponent
{
    protected override IEnumerator Coroutine_PerformEnterActions()
    {
        LevelManager.Instance.RegisterScene(Constants.Enumerations.SceneType.Environment, Constants.SceneName.Environment_Safehouse);
        GameStateManager.Instance.SetEnvironment(Constants.Enumerations.Environment.Safehouse);
        GameStateManager.Instance.ToGameplay();

        yield return base.Coroutine_PerformEnterActions();
    }

    protected override void ExitActions()
    {
        GameStateManager.Instance.SetEnvironment(Constants.Enumerations.Environment.None);

        base.ExitActions();
    }
}
