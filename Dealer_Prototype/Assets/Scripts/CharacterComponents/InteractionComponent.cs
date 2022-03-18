using Constants;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class InteractionComponent : MonoBehaviour, IInteraction
{
    public CharacterComponent CharPtr;

    public delegate void OnMouseEnterEvent();
    public delegate void OnMouseExitEvent();
    public delegate void OnMouseClickedEvent();

    public OnMouseEnterEvent MouseEnterEvent;
    public OnMouseExitEvent MouseExitEvent;
    public OnMouseClickedEvent MouseClickedEvent;

    private bool bMouseIsOver = false;

    public virtual void MouseEnter()
    {
        if (PlayableCharacterManager.Instance.IsCharacterCurrentlySelected() && PlayableCharacterManager.Instance.GetSelectedCharacter() != CharPtr)
        {
            GameplayCanvas.Instance.SetInteractionTipTextContext(InteractableConstants.InteractionContext.Talk);

            CursorManager.Instance.ToInteract();
        }
    }

    public virtual void MouseClick()
    {
        if (MouseClickedEvent != null) MouseClickedEvent();
    }

    private void FixedUpdate()
    {
        if (Camera.main == null) return;

        Vector2 _screenMousePos = InputManager.Instance.GetScreenMousePosition();

        var ray = Camera.main.ScreenPointToRay(_screenMousePos);

        RaycastHit hit = new RaycastHit();

        //for walls, interactables, doors, etc
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            IInteraction interactionInterface = hit.collider.GetComponent<IInteraction>();
            if (interactionInterface != null)
            {
                if (hit.collider.gameObject == this.gameObject && bMouseIsOver == false)
                {
                    MouseEnter();
                    bMouseIsOver = true;
                }
            }
            else if (bMouseIsOver)
            {
                bMouseIsOver = false;
                MouseExit();
            }
        }
        else if (bMouseIsOver)
        {
            bMouseIsOver = false;
            MouseExit();
        }
    }

    public void MouseExit() { }
}
