using UnityEngine.InputSystem;
using UnityEngine;
using Constants;

public class InputManager : MonoBehaviour
{
    private PlayerInputActions inputActions;

    private Vector2 _screenMousePos;

    //singleton stuff 

    private static InputManager _instance;

    public static InputManager Instance { get { return _instance; } }

    private IInterior Tracked_MouseOver_Interior = null;

    const int ground_layerMask = 1 << 6;

    bool bRevealStarted = false;


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

        inputActions.Default.Select.performed += ctx => OnSelect(ctx);
        inputActions.Default.Cancel.performed += ctx => OnCancel(ctx);

        inputActions.Default.Reveal.started += ctx => Reveal_Start(ctx);

        inputActions.Enable();
    }

    private void Reveal_Start(InputAction.CallbackContext context)
    {
        InteractableManager.Instance.ToggleHighlightAll(true);
        bRevealStarted = true;
    }

    private void Reveal_Complete()
    {
        InteractableManager.Instance.ToggleHighlightAll(false);
    }


    private void FixedUpdate()
    {
        if (GameState.Instance.GetState() == GameState.State.GamePlay)
        {

            InteractableConstants.InteractionContext context = InteractableConstants.InteractionContext.None;

            HandleKeyboard();

            context = HandleMouse();
            context = HandleInteractables();

            UIManager.Instance.HandleEvent(context);

        }
        else if (GameState.Instance.GetState() == GameState.State.Conversation)
        {

        }
    }

    private InteractableConstants.InteractionContext HandleInteractables()
    {
        InteractableConstants.InteractionContext context = InteractableConstants.InteractionContext.None;

        foreach (InteractionUpdateResult result in InteractableManager.Instance.PerformUpdates())
        {
            if (result.success)
            {
                if (PlayableCharacterManager.Instance)
                {
                    if (PlayableCharacterManager.Instance.IsCharacterCurrentlySelected())
                    {
                        if (result.context != InteractableConstants.InteractionContext.None)
                        {
                            context = result.context;
                        }

                    }

                    CursorManager.Instance.ToInteract();

                    result.interactable.ToggleOutlineShader(true);
                }
            }
            else
            {
                result.interactable.ToggleOutlineShader(false);
            }
        }

        return context;
    }


    private InteractableConstants.InteractionContext HandleMouse()
    {
        InteractableConstants.InteractionContext context = InteractableConstants.InteractionContext.None;

        _screenMousePos = inputActions.Default.Aim.ReadValue<Vector2>();

        var ray = Camera.main.ScreenPointToRay(_screenMousePos);

        RaycastHit hit = new RaycastHit();

        bool mouseEvent = false;

        //for walls, doors, etc
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, ~ground_layerMask))
        {
            IInterior interiorInterface = hit.collider.GetComponent<IInterior>();
            if (interiorInterface != null)
            {
                if (Tracked_MouseOver_Interior != null && Tracked_MouseOver_Interior != interiorInterface)
                {
                    Tracked_MouseOver_Interior.MouseExit();
                }

                Tracked_MouseOver_Interior = interiorInterface;
                context = Tracked_MouseOver_Interior.MouseEnter();

                mouseEvent = true;
            }
        }
        else if (Tracked_MouseOver_Interior != null)
        {
            Tracked_MouseOver_Interior.MouseExit();
        }

        //for floor/ground tiles
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, ground_layerMask))
        {
            IInterior interiorInterface = hit.collider.GetComponent<IInterior>();
            if (interiorInterface != null)
            {
                context = interiorInterface.MouseEnter();

                mouseEvent = true;
            }
        }

        if (!mouseEvent)
        {
            ClearMouseExitTarget();
            UIManager.Instance.HandleEvent(UI.Events.Clear);
            CursorManager.Instance.ToCancel();
        }

        return context;
    }

    private void ClearMouseExitTarget()
    {
        if (Tracked_MouseOver_Interior != null)
        {
            Tracked_MouseOver_Interior.MouseExit();
            Tracked_MouseOver_Interior = null;
        }
    }

    private void HandleKeyboard()
    {
        //reading the input:
        Vector2 InputVector = Vector2.zero;
        InputVector.x = (-1 * inputActions.Default.Left.ReadValue<float>()) + inputActions.Default.Right.ReadValue<float>();
        InputVector.y = (-1 * inputActions.Default.Down.ReadValue<float>()) + inputActions.Default.Up.ReadValue<float>();

        CameraFollowTarget.Instance.MoveInDirection(InputVector);

        if (bRevealStarted && inputActions.Default.Reveal.ReadValue<float>() == 0)
        {
            Reveal_Complete();
        }
    }

    private void OnSelect(InputAction.CallbackContext context)
    {
        if (GameState.Instance.GetState() != GameState.State.GamePlay) { return; }

        DebugManager.Instance.Print(DebugManager.Log.LogInput, "Pointer click at " + _screenMousePos);

        var ray = Camera.main.ScreenPointToRay(_screenMousePos);

        RaycastHit hit = new RaycastHit();

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, ~ground_layerMask))
        {
            DebugManager.Instance.Print(DebugManager.Log.LogInput, "Ray hit at " + hit.point);

            IInteraction interactionInterface = hit.collider.GetComponent<IInteraction>();
            IInterior interiorInterface = hit.collider.GetComponent<IInterior>();
            if (interactionInterface != null)
            {
                interactionInterface.MouseClick();
                return;
            }
            else if (interiorInterface != null)
            {
                interiorInterface.MouseClick(hit.point);
            }

            CameraFollowTarget.Instance.MoveTo(hit.point);
        }
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, ground_layerMask))
        {
            DebugManager.Instance.Print(DebugManager.Log.LogInput, "Ray hit ground at " + hit.point);

            IInterior interiorInterface = hit.collider.GetComponent<IInterior>();
            if (interiorInterface != null)
            {
                interiorInterface.MouseClick(hit.point);
            }

            CameraFollowTarget.Instance.MoveTo(hit.point);
        }
    }

    private void OnCancel(InputAction.CallbackContext context)
    {
        if (GameState.Instance.GetState() != GameState.State.GamePlay) { return; }

        PlayableCharacterManager.Instance.AttemptBehaviorAbortWithPossesedCharacter();
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

    public Vector2 GetScreenMousePosition() { return _screenMousePos; }

    public InteractionUpdateResult PerformInteractableUpdate(IInteraction interactable)
    {
        InteractionUpdateResult result = InteractionUpdateResult.Build();
        result.context = interactable.GetContext();
        result.interactable = interactable;

        if (Camera.main == null) return result;

        Vector2 _screenMousePos = GetScreenMousePosition();

        var ray = Camera.main.ScreenPointToRay(_screenMousePos);
        _ = new RaycastHit();
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            IInteraction interactionInterface = hit.collider.GetComponent<IInteraction>();
            if (interactionInterface != null && hit.collider.gameObject == interactable.GetGameObject())
            {
                result.success = true;
            }
        }

        return result;
    }
}
