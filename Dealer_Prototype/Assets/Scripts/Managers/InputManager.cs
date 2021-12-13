using UnityEngine.InputSystem;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private static InputManager _instance;

    private PlayerInputActions inputActions;

    private Vector2 _screenMousePos;

    public enum InputDirection { Up, Down, Left, Right };

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

        inputActions.Default.RotateClockwise.performed += ctx => CameraManager.Instance.RotateClockwise();
        inputActions.Default.RotateCounterClockwise.performed += ctx => CameraManager.Instance.RotateCounterClockwise();

        inputActions.Default.ZoomOut.performed += ctx => CameraManager.Instance.ZoomOut();
        inputActions.Default.ZoomIn.performed += ctx => CameraManager.Instance.ZoomIn();

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
                PlayerComponent.Instance.MoveToLocation(hit.point);
            }
        }
    }

    private void FixedUpdate()
    {
        HandleKeyBoard();
        HandleMouse();
    }

    private void HandleKeyBoard()
    {
        if(inputActions.Default.Up.ReadValue<float>() > 0)
        {
            CameraController.Instance.MoveInDirection(InputDirection.Up);
        }
        if (inputActions.Default.Down.ReadValue<float>() > 0)
        {
            CameraController.Instance.MoveInDirection(InputDirection.Down);
        }
        if (inputActions.Default.Left.ReadValue<float>() > 0)
        {
            CameraController.Instance.MoveInDirection(InputDirection.Left);
        }
        if (inputActions.Default.Right.ReadValue<float>() > 0)
        {
            CameraController.Instance.MoveInDirection(InputDirection.Right);
        }
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
                mouseEvent = true;
            }
        }

        if (!mouseEvent)
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
