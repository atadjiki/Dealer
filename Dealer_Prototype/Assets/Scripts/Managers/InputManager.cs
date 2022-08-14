using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Constants;

public class InputManager : Singleton<InputManager>, IEventReceiver
{
    [SerializeField] private KeyCode key_pause = KeyCode.Tab;
    [SerializeField] private KeyCode key_debugMenu = KeyCode.Tilde;

    public void HandleEvent(Enumerations.EventID eventID)
    {
    }

    private void Update()
    {
        if (Input.GetKeyDown(key_pause))
        {
            if (debug) Debug.Log("key press " + key_pause);
            GameStateManager.Instance.TogglePause();
        }
        if(Input.GetKeyDown(key_debugMenu))
        {
            if(debug) Debug.Log("ley press " + key_debugMenu);
            LevelManager.Instance.ToggleDebugMenu();
            
        }
    }
}
