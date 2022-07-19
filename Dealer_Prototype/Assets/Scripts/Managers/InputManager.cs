using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Constants;

public class InputManager : Singleton<InputManager>
{
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            if(GameStateManager.Instance.GetGameMode() == Enumerations.GameMode.GamePlay)
            {
                GameStateManager.Instance.Pause();
            }
            else
            {
                GameStateManager.Instance.ToGameplay();
            }
        }
        else if(Input.GetKeyDown(KeyCode.M))
        {
            GameStateManager.Instance.AdjustPlayerMoney(1);
        }
    }
}
