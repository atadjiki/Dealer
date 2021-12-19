using UnityEngine.InputSystem;
using UnityEngine;

public class InputManager : MonoBehaviour
{ 
    private PlayerInputActions inputActions;

    private Vector2 _screenMousePos;

    //singleton stuff 

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
        inputActions = new PlayerInputActions();

        inputActions.Default.Select.performed += ctx => OnMouseActionPerformed(ctx);

        inputActions.Enable();
    }


    private void FixedUpdate()
    {
        HandleKeyboard();
        HandleMouse();
    }


    private void HandleMouse()
    {
        _screenMousePos = inputActions.Default.Aim.ReadValue<Vector2>();

        var ray = Camera.main.ScreenPointToRay(_screenMousePos);

        RaycastHit hit = new RaycastHit();

        bool mouseEvent = false;

        if (Physics.Raycast(ray, out hit))
        {
            IInteraction interactionInterface = hit.collider.GetComponent<IInteraction>();
            if (interactionInterface != null)
            {
                interactionInterface.MouseEnter();
                CursorManager.Instance.ToInteract();
                mouseEvent = true;
            }
            else if (hit.collider.tag == "Ground")
            {
                if (NPCManager.Instance.IsNPCCurrentlySelected())
                    GameplayCanvas.Instance.SetInteractionTipTextContext(GameplayCanvas.InteractionContext.Move);
                else
                    GameplayCanvas.Instance.ClearInteractionTipText();

                CursorManager.Instance.ToDefault();
                mouseEvent = true;
            }
        }

        if (!mouseEvent)
        {
            GameplayCanvas.Instance.ClearInteractionTipText();
            CursorManager.Instance.ToCancel();
        }
    }

    private void HandleKeyboard()
    {
        //reading the input:
        Vector2 InputVector = Vector2.zero;
        InputVector.x = (-1*inputActions.Default.Left.ReadValue<float>()) + inputActions.Default.Right.ReadValue<float>();
        InputVector.y = (-1*inputActions.Default.Down.ReadValue<float>()) + inputActions.Default.Up.ReadValue<float>();

        CameraFollowTarget.Instance.MoveInDirection(InputVector);
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
                NPCManager.Instance.AttemptMoveOnPossesedNPC(hit.point);
            }

            CameraFollowTarget.Instance.MoveTo(hit.point);
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
