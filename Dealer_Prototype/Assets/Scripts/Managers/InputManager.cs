using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Constants;

public class InputManager : Singleton<InputManager>
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (debug) Debug.Log("key press " + KeyCode.Tab);
            GameStateManager.Instance.TogglePause();
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            if (debug) Debug.Log("key press " + KeyCode.Space);
            GameStateManager.Instance.ToSafehouse();
        }
    }
}
