using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Environment_Safehouse : EnvironmentComponent
{
    protected override void EnterActions()
    {
        base.EnterActions();

        if(GameStateManager.Instance != null)
        {
            GameStateManager.Instance.ToGameplay();
        }
    }

    protected override void ExitActions()
    {
        base.ExitActions();
    }
}
