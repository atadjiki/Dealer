using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine;

public class InteractableManager : MonoBehaviour
{
    private static InteractableManager _instance;

    public static InteractableManager Instance { get { return _instance; } }

    private List<IInteraction> Interactables;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }

        Build();
    }

    private void Build()
    {
        Interactables = new List<IInteraction>();
    }

    public bool Register(IInteraction interactable)
    {
        Interactables.Add(interactable);

        DebugManager.Instance.Print(DebugManager.Log.LogInteractableManager, "Registered Interactable " + interactable.GetID());
        return true;
    }

    public void UnRegister(IInteraction interactable)
    {
        DebugManager.Instance.Print(DebugManager.Log.LogInteractableManager, "Unregistered Interactable " + interactable.GetID());
        Interactables.Remove(interactable);
    }

    public bool FindInteractableByID(InteractableConstants.InteractableID ID, out IInteraction result)
    {
        foreach (IInteraction interactable in Interactables)
        {
            if (interactable.GetID() == ID.ToString())
            {
                result = interactable;
                return true;
            }
        }

        result = null;
        return false;
    }

    public void ToggleHighlightAll(bool flag)
    {
        foreach(IInteraction interactable in Interactables)
        {
            interactable.ToggleOutlineShader(flag);
        }
    }

    public InteractionUpdateResult[] PerformUpdates()
    {
        List<InteractionUpdateResult> results = new List<InteractionUpdateResult>();

        foreach (IInteraction interactable in Interactables)
        {
            if(interactable.IsVisible())
            {
                results.Add(InputManager.Instance.PerformInteractableUpdate(interactable));
            }
        }

        return results.ToArray();
    }
}
