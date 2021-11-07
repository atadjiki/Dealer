using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private static InputManager _instance;

    public static InputManager Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }

        Build();
    }

    private void Build()
    {
        
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if(DebugManager.Instance.LogInput) Debug.Log("Pointer click at " + Input.mousePosition);

            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit = new RaycastHit();

            if (Physics.Raycast(ray, out hit))
            {
                //if the mouse just hit the ground, move to the specified location
                if (hit.collider.tag == "Ground")
                {
                    if(PlayerComponent.Instance != null)
                        PlayerComponent.Instance._navigator.MoveToLocation(hit.point);
                }
            }
        }
    }

    public void LockControls()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void UnlockControls()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
