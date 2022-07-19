using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Constants;

public class InputManager : Singleton<InputManager>
{
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            GameStateManager.Instance.TogglePause();

            if (debug) Debug.Log("Key press " + KeyCode.Tab);
        }
        else if(Input.GetKeyDown(KeyCode.Space))
        {
            GameStateManager.Instance.ToSafehouse();

            if (debug) Debug.Log("Key press " + KeyCode.Space);
        }
    }
}
