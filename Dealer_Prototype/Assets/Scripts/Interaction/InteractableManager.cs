using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine;

public class InteractableManager : MonoBehaviour
{
    private static InteractableManager _instance;

    public static InteractableManager Instance { get { return _instance; } }

    private List<Interactable> Interactables;

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
        Interactables = new List<Interactable>();
    }

    public bool Register(Interactable interactable)
    {
        Interactables.Add(interactable);

        DebugManager.Instance.Print(DebugManager.Log.LogInteractableManager, "Registered Interactable " + interactable.GetID());
        return true;
    }

    public void UnRegister(Interactable interactable)
    {
        DebugManager.Instance.Print(DebugManager.Log.LogInteractableManager, "Unregistered Interactable " + interactable.GetID());
        Interactables.Remove(interactable);
    }

    public bool FindInteractableByID(InteractableConstants.InteractableID ID, out Interactable result)
    {
        foreach (Interactable interactable in Interactables)
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
        foreach(Interactable interactable in Interactables)
        {
            interactable.ToggleOutlineShader(flag);
        }
    }
}
