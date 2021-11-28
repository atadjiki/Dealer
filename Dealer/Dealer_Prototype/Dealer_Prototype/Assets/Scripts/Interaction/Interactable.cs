using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Interactable : MonoBehaviour
{
    private InteractableStateComponent _interactionState;
    [SerializeField] private InteractableConstants.InteractableID ID;

    internal InteractableConstants.InteractionState CurrentState;

    public InteractableConstants.InteractionState GetState() { return CurrentState; }
    public void SetState(InteractableConstants.InteractionState newState) { CurrentState = newState; }

    internal NPCComponent _interactee = null;
    internal HashSet<NPCComponent> _interactedWith;

    [SerializeField] internal Transform InteractionTransform;

    public Transform GetInteractionTransform() { return InteractionTransform; }

    private void Awake()
    {
        StartCoroutine(DoInitialize());
    }

    private IEnumerator DoInitialize()
    {
        if (NPCManager.Instance.RegisterInteractable(this) == false)
        {
            Destroy(this.gameObject);
        }

        _interactedWith = new HashSet<NPCComponent>();

        _interactionState = this.gameObject.AddComponent<InteractableStateComponent>();
        _interactionState.SetInteractableID(ID);

        SetState(InteractableConstants.InteractionState.Available);

        yield return null;

    }

    private void OnDestroy()
    {
        NPCManager.Instance.UnRegisterInteractable(this);
    }

    public bool HasBeenInteractedWith(NPCComponent npc)
    {
        return _interactedWith.Contains(npc);
    }

    public virtual void OnMouseEnter()
    {
        GameplayCanvas.Instance.SetInteractionTipText(this);
    }

    public virtual void OnMouseExit()
    {
        GameplayCanvas.Instance.ClearInteractionTipText();
    }

    public virtual void OnMouseClicked() { }

    internal virtual void Interaction(NPCComponent interactee)
    {
        SetState(InteractableConstants.InteractionState.Busy);

        StartCoroutine(DoInteraction());
    }

    internal virtual IEnumerator DoInteraction()
    {
        SetState(InteractableConstants.InteractionState.Available);
        yield return null;
    }

    public string GetID()
    {
        return _interactionState.GetID();
    }
}
