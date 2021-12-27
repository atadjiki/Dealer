using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{ 
    private PlayerInputActions inputActions;

    private Vector2 _screenMousePos;

    //singleton stuff 

    private static InputManager _instance;

    public static InputManager Instance { get { return _instance; } }

    private IInterior Tracked_MouseOver_Interior = null;

    const int ground_layerMask = 1 << 6;


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

        //for walls, interactables, doors, etc
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, ~ground_layerMask))
        {
            IInteraction interactionInterface = hit.collider.GetComponent<IInteraction>();
            IInterior interiorInterface = hit.collider.GetComponent<IInterior>();
            if (interactionInterface != null)
            {
                interactionInterface.MouseEnter();
                mouseEvent = true;
            }
            else if (interiorInterface != null)
            {
                if (Tracked_MouseOver_Interior != null && interiorInterface == Tracked_MouseOver_Interior) { return; }

                if (Tracked_MouseOver_Interior != null && Tracked_MouseOver_Interior != interiorInterface)
                {
                    Tracked_MouseOver_Interior.MouseExit();
                }

                Tracked_MouseOver_Interior = interiorInterface;
                Tracked_MouseOver_Interior.MouseEnter();

                mouseEvent = true;
            }
        }

        //for floor/ground tiles
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, ground_layerMask))
        {
            IInterior interiorInterface = hit.collider.GetComponent<IInterior>();
            if (interiorInterface != null)
            {
                interiorInterface.MouseEnter();

                mouseEvent = true;
            }
        }

        if (!mouseEvent)
        {
            ClearMouseExitTarget();
            GameplayCanvas.Instance.ClearInteractionTipText();
            CursorManager.Instance.ToCancel();
        }
    }

    private void ClearMouseExitTarget()
    {
        if(Tracked_MouseOver_Interior != null)
        {
            Tracked_MouseOver_Interior.MouseExit();
            Tracked_MouseOver_Interior = null;
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
        DebugManager.Instance.Print(DebugManager.Log.LogInput, "Pointer click at " + _screenMousePos);

        var ray = Camera.main.ScreenPointToRay(_screenMousePos);

        RaycastHit hit = new RaycastHit();

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, ground_layerMask))
        {
            DebugManager.Instance.Print(DebugManager.Log.LogInput, "Ray hit ground at " + hit.point);

            IInterior interiorInterface = hit.collider.GetComponent<IInterior>();
            if(interiorInterface != null)
            {
                interiorInterface.MouseClick(hit.point);
            }
            
            CameraFollowTarget.Instance.MoveTo(hit.point);
        }
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, ~ground_layerMask))
        {
            DebugManager.Instance.Print(DebugManager.Log.LogInput, "Ray hit at " + hit.point);

            IInteraction interactionInterface = hit.collider.GetComponent<IInteraction>();
            IInterior interiorInterface = hit.collider.GetComponent<IInterior>();
            if (interactionInterface != null)
            {
                interactionInterface.MouseClick();
            }
            else if (interiorInterface != null)
            {
                interiorInterface.MouseClick(hit.point);
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
