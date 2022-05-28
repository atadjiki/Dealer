using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Interactable : MonoBehaviour
{
    [SerializeField] private InteractableConstants.InteractableID ID;

    internal InteractableConstants.InteractionState CurrentState;

    public InteractableConstants.InteractionState GetState() { return CurrentState; }
    public void SetState(InteractableConstants.InteractionState newState) { CurrentState = newState; }

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
        InteractableManager.Instance.UnRegister(this);
    }

    protected virtual IEnumerator DoInitialize()
    {
        SetState(InteractableConstants.InteractionState.Available);

        if (InteractableManager.Instance.Register(this) == false)
        {
            Destroy(this.gameObject);
        }

        yield return null;
    }

    public void ToggleOutlineShader(bool flag)
    {
        if (flag)
        {
            foreach (MeshRenderer renderer in Meshes)
            {
                MaterialManager.Instance.ApplyOutlineMaterialToMesh(renderer);
            }
        }
        else
        {
            foreach (MeshRenderer renderer in Meshes)
            {
                MaterialManager.Instance.RemoveOutlineMaterialFromMesh(renderer);
            }
        }
    }

    public string GetID()
    {
        return ID.ToString();
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

    public bool IsVisible()
    {
        foreach (MeshRenderer mesh in Meshes)
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
