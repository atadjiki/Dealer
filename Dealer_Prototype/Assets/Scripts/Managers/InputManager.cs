using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Constants;

public class InputManager : Singleton<InputManager>, IEventReceiver
{
    [SerializeField] private KeyCode key_pause = KeyCode.Tab;

    public void HandleEvent(Enumerations.EventID eventID)
    {
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (debug) Debug.Log("key press " + key_pause);
            GameStateManager.Instance.TogglePause();
        }
    }
}
