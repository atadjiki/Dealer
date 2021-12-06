using UnityEngine.InputSystem;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private static InputManager _instance;

    private PlayerInputActions inputActions;

    private Vector2 _screenMousePos;

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
        inputActions = new PlayerInputActions();

        inputActions.Mouse.Select.performed += ctx => OnMouseActionPerformed(ctx);

        inputActions.Enable();
    }


    private void OnMouseActionPerformed(InputAction.CallbackContext context)
    {
        if (DebugManager.Instance.LogInput) Debug.Log("Pointer click at " + _screenMousePos);

        var ray = Camera.main.ScreenPointToRay(_screenMousePos);

        RaycastHit hit = new RaycastHit();

        if (Physics.Raycast(ray, out hit))
        {
            if (DebugManager.Instance.LogInput) Debug.Log("Ray hit ground at " + hit.point);

            IInteraction interactionInterface = hit.collider.GetComponent<IInteraction>();
            if (interactionInterface != null)
            {
                interactionInterface.MouseClick();
            }
            //if the mouse just hit the ground, move to the specified location
            else if (hit.collider.tag == "Ground")
            {
                if (NPCManager.Instance.GetSelectedNPC() != null)
                {
                    NPCManager.Instance.AttemptMoveOnPossesedNPC(hit.point);
                }
            }

        }
    }

    private void FixedUpdate()
    {
        _screenMousePos = inputActions.Mouse.Aim.ReadValue<Vector2>();

        var ray = Camera.main.ScreenPointToRay(_screenMousePos);

        RaycastHit hit = new RaycastHit();

        bool mouseEvent = false;

        if (Physics.Raycast(ray, out hit))
        {
            IInteraction interactionInterface = hit.collider.GetComponent<IInteraction>();
            if (interactionInterface != null)
            {
                interactionInterface.MouseEnter();
                mouseEvent = true;
            }
        }

        if(!mouseEvent)
        {
            GameplayCanvas.Instance.ClearInteractionTipText();
        }
    }

    public void LockControls()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        inputActions.Disable();
    }

    public void UnlockControls()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        inputActions.Enable();
    }
}
