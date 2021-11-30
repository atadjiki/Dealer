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

    internal HashSet<NPCComponent> _interactedWith;

    [SerializeField] internal Transform InteractionTransform;

    public Transform GetInteractionTransform() { return InteractionTransform; }

    private void Awake()
    {
        StartCoroutine(DoInitialize());
    }

    private IEnumerator DoInitialize()
    {
        _interactedWith = new HashSet<NPCComponent>();

        _interactionState = this.gameObject.AddComponent<InteractableStateComponent>();

        yield return new WaitUntil(() => _interactionState != null);

        _interactionState.SetInteractableID(ID);

        SetState(InteractableConstants.InteractionState.Available);

        if (NPCManager.Instance.RegisterInteractable(this) == false)
        {
            Destroy(this.gameObject);
        }

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

    public string GetID()
    {
        if (_interactionState != null)
            return _interactionState.GetID();
        else
            return "";
    }
}
