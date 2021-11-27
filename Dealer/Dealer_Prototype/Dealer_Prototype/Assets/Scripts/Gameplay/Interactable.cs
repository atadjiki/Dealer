using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    private InteractionComponent _interaction;
    private InteractableStateComponent _interactionState;
    [SerializeField] private InteractableConstants.InteractableID ID;


    private void Awake()
    {
        StartCoroutine(DoInitialize());
    }

    private IEnumerator DoInitialize()
    {
        _interactionState = this.gameObject.AddComponent<InteractableStateComponent>();
        _interactionState.SetInteractableID(ID);

        GameObject InteractionPrefab = PrefabFactory.Instance.CreatePrefab(RegistryID.Interaction, this.transform);
        _interaction = InteractionPrefab.GetComponent<InteractionComponent>();

        yield return new WaitWhile(() => _interaction == null);

        _interaction.MouseEnterEvent += OnMouseEnter;
        _interaction.MouseExitEvent += OnMouseExit;
        _interaction.MouseClickedEvent += OnMouseClicked;
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
        return _interactionState.GetID();
    }
}
