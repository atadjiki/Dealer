using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine.EventSystems;
using Pathfinding;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Interactable : MonoBehaviour, IInteraction
{
    private InteractableStateComponent _interactionState;
    [SerializeField] private InteractableConstants.InteractableID ID;

    internal InteractableConstants.InteractionState CurrentState;

    public InteractableConstants.InteractionState GetState() { return CurrentState; }
    public void SetState(InteractableConstants.InteractionState newState) { CurrentState = newState; }

    internal HashSet<CharacterComponent> _interactedWith;

    [SerializeField] internal Transform InteractionTransform;
    [SerializeField] internal MeshRenderer[] Meshes;

    public Transform GetInteractionTransform() { return InteractionTransform; }

    private bool bMouseIsOver = false;

    private void Awake()
    {
        StartCoroutine(DoInitialize());
    }

    private IEnumerator DoInitialize()
    {
        _interactedWith = new HashSet<CharacterComponent>();

        _interactionState = this.gameObject.AddComponent<InteractableStateComponent>();

        yield return new WaitUntil(() => _interactionState != null);

        _interactionState.SetInteractableID(ID);

        SetState(InteractableConstants.InteractionState.Available);

        if (InteractableManager.Instance.Register(this) == false)
        {
            Destroy(this.gameObject);
        }

        yield return null;

    }

    private void OnDestroy()
    {
        InteractableManager.Instance.UnRegister(this);
    }

    public bool HasBeenInteractedWith(CharacterComponent character)
    {
        return _interactedWith.Contains(character);
    }

    private void FixedUpdate()
    {
        Vector2 _screenMousePos = InputManager.Instance.GetScreenMousePosition();

        var ray = Camera.main.ScreenPointToRay(_screenMousePos);

        RaycastHit hit = new RaycastHit();

        //for walls, interactables, doors, etc
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            IInteraction interactionInterface = hit.collider.GetComponent<IInteraction>();
            if (interactionInterface != null)
            {
                if(hit.collider.gameObject == this.gameObject && bMouseIsOver == false)
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
        else if(bMouseIsOver)
        {
            bMouseIsOver = false;
            MouseExit();
        }
    }

    public void ToggleOutlineShader(bool flag)
    {
        if(flag)
        {
            foreach (MeshRenderer renderer in Meshes)
            {
                ColorManager.Instance.ApplyOutlineMaterialToMesh(renderer);
            }
        }
        else
        {
            foreach (MeshRenderer renderer in Meshes)
            {
                ColorManager.Instance.RemoveOutlineMaterialFromMesh(renderer);
            }
        }
    }

    public virtual void MouseEnter()
    {
        if (PlayableCharacterManager.Instance.IsCharacterCurrentlySelected())
        {
            GameplayCanvas.Instance.SetInteractionTipTextContext(GameplayCanvas.GetContextByInteractableID(this));
        }

        CursorManager.Instance.ToInteract();

        ToggleOutlineShader(true);
    }

    public virtual void MouseClick()
    {
        if (PlayableCharacterManager.Instance.GetSelectedCharacter() != null)
        {
            PlayableCharacterManager.Instance.AttemptInteractWithPossesedCharacter(this);
        }
    }

    public string GetID()
    {
        if (_interactionState != null)
            return _interactionState.GetID();
        else
            return "";
    }

    public void MouseExit()
    {
        ToggleOutlineShader(false);
    }
}
