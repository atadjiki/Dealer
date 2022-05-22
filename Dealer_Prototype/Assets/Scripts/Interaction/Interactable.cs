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

    internal HashSet<CharacterComponent> _interactedWith;

    [SerializeField] internal Transform InteractionTransform;
    [SerializeField] internal MeshRenderer[] Meshes;

    [SerializeField] internal GameObject uiPanelPrefab;

    public Transform GetInteractionTransform() { return InteractionTransform; }

    private void Awake()
    {
        StartCoroutine(DoInitialize());
    }

    private void OnDestroy()
    {
        if (InteractableManager.Instance)
        {
            InteractableManager.Instance.UnRegister(this);
        }
    }

    protected virtual IEnumerator DoInitialize()
    {
        if(InteractableManager.Instance)
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

            if (InfoPanelManager.Instance && uiPanelPrefab)
            {
                InfoPanelManager.Instance.RegisterInteractable(this, uiPanelPrefab);
            }
            else if(InfoPanelManager.Instance == null)
            {
                Debug.Log("could not register " + this.name + ", infopanel manager is null");
            }
        }
        else
        {
            Debug.Log("could not initialize" + this.name + ", interactable manager is null");
        }


        yield return null;

    }

    public bool HasBeenInteractedWith(CharacterComponent character)
    {
        return _interactedWith.Contains(character);
    }

    public void ToggleOutlineShader(bool flag)
    {
        if(ColorManager.Instance)
        {
            if (flag)
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
    }

    public string GetID()
    {
        if (_interactionState != null)
            return _interactionState.GetID();
        else
            return "";
    }

    private void OnMouseOver()
    {
        CursorManager.Instance.ToInteract();
        
    }

    private void OnMouseEnter()
    {
        ToggleOutlineShader(true);
    }

    private void OnMouseExit()
    {
        ToggleOutlineShader(false);
        CursorManager.Instance.ToDefault();
    }

    public bool HasBeenInteractedWith(NPCComponent npc)
    {
        return false;   
    }

    public virtual void OnInteraction()
    {

    }

    public bool IsVisible()
    {
        foreach(MeshRenderer mesh in Meshes)
        {
            if (mesh.isVisible)
            {
                return true;
            }
        }

        return false;
    }

    public InteractableConstants.InteractionContext GetContext()
    {
        return InteractableConstants.GetContextByInteractableID(this);
    }

    public GameObject GetGameObject()
    {
        return this.gameObject;
    }
}