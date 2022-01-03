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

    public Transform GetInteractionTransform() { return InteractionTransform; }

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


    public virtual void MouseEnter()
    {
        if (PlayableCharacterManager.Instance.IsCharacterCurrentlySelected())
        {
           // if(BehaviorHelper.IsInteractionAllowed(PlayableCharacterManager.Instance.GetSelectedCharacter(), this))
         //   {
                GameplayCanvas.Instance.SetInteractionTipTextContext(GameplayCanvas.GetContextByInteractableID(this));
         //   } 
        }

        CursorManager.Instance.ToInteract();
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

    public void MouseExit() { }
}
