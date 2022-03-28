using System.Collections.Generic;
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

    //private bool bMouseIsOver = false;

    private List<SkinnedMeshRenderer> Meshes;

    public void SetMeshes(SkinnedMeshRenderer[] meshArray)
    {
        Meshes = new List<SkinnedMeshRenderer>(meshArray);
    }

    public virtual void MouseClick()
    {
        if (MouseClickedEvent != null) MouseClickedEvent();
    }

    public void ToggleOutlineShader(bool flag)
    {
        if (ColorManager.Instance)
        {
            if (flag)
            {
                foreach (SkinnedMeshRenderer renderer in Meshes)
                {
                    ColorManager.Instance.ApplyOutlineMaterialToMesh(renderer);
                }
            }
            else
            {
                foreach (SkinnedMeshRenderer renderer in Meshes)
                {
                    ColorManager.Instance.RemoveOutlineMaterialFromMesh(renderer);
                }
            }
        }
    }

    public string GetID()
    {
        if (CharPtr != null)
        {
            return CharPtr.GetID();
        }
        else
        {
            return null;
        }
    }

    public bool HasBeenInteractedWith(NPCComponent npc)
    {
        return false;
    }

    public void OnInteraction()
    {

    }

    public Transform GetInteractionTransform()
    {
        return CharPtr.GetNavigatorComponent().transform;
    }

    public bool IsVisible()
    {
        if(Meshes == null) { return false; }

        foreach(SkinnedMeshRenderer mesh in Meshes)
        {
            if(mesh.isVisible)
            {
                return true;
            }
        }

        return false;
    }

    public InteractableConstants.InteractionContext GetContext()
    {
        if(CharPtr == PlayableCharacterManager.Instance.GetSelectedCharacter())
        {
            return InteractableConstants.InteractionContext.None;
        }
        else
        {
            return InteractableConstants.InteractionContext.Talk;
        }
    }

    public GameObject GetGameObject()
    {
        return this.gameObject;
    }
}
