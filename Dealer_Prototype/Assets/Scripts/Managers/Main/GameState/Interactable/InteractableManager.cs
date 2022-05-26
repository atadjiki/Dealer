using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine;

public class InteractableManager : Manager
{
    private static InteractableManager _instance;

    public static InteractableManager Instance { get { return _instance; } }

    private List<Interactable> Interactables;

    public delegate void OnInteractableRegistered(Interactable interactable);
    public delegate void OnInteractableUnRegistered(Interactable interactable);
    public delegate void OnInteractableManagerUpdate();

    public OnInteractableRegistered onInteractableRegistered;
    public OnInteractableUnRegistered onInteractableUnRegistered;
    public OnInteractableManagerUpdate onInteractableManagerUpdate;

    public override void Build()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }

        Interactables = new List<Interactable>();

        base.Build();
    }

    public bool Register(Interactable interactable)
    {
        Interactables.Add(interactable);
        onInteractableRegistered(interactable);

        DebugManager.Instance.Print(DebugManager.Log.LogInteractableManager, "Registered Interactable " + interactable.GetID());
        return true;
    }

    public void UnRegister(Interactable interactable)
    {
        DebugManager.Instance.Print(DebugManager.Log.LogInteractableManager, "Unregistered Interactable " + interactable.GetID());
        onInteractableUnRegistered(interactable);
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
}
