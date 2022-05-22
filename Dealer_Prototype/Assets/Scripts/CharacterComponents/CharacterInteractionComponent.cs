using System.Collections.Generic;
using Constants;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class CharacterInteractionComponent : MonoBehaviour
{
    public CharacterComponent CharPtr;

    private List<SkinnedMeshRenderer> Meshes;

    public void SetMeshes(SkinnedMeshRenderer[] meshArray)
    {
        Meshes = new List<SkinnedMeshRenderer>(meshArray);
    }

    private void OnMouseEnter()
    {
        ToggleOutlineShader(true);
    }

    private void OnMouseExit()
    {
        ToggleOutlineShader(false);   
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
        return InteractableConstants.InteractionContext.None;
    }

    public GameObject GetGameObject()
    {
        return this.gameObject;
    }
}
