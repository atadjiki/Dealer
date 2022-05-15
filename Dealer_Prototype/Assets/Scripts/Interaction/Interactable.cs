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
       // Debug.Log(this.gameObject.name);
    }

    private void OnMouseExit()
    {
        ToggleOutlineShader(false);
        CursorManager.Instance.ToDefault();
    }

    private void OnMouseDown()
    {
        if (PlayableCharacterManager.Instance)
        {
            if (PlayableCharacterManager.Instance.GetSelectedCharacter() != null)
            {
                PlayableCharacterManager.Instance.AttemptInteractWithPossesedCharacter(this);
            }
        }

    //    Debug.Log(this.name + " clicked.");
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
