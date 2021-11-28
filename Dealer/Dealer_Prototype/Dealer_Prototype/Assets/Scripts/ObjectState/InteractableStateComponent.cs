using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine;

public class InteractableStateComponent : ObjectStateComponent
{
    [Header("Interactable ID")]
    [SerializeField] private InteractableConstants.InteractableID InteractableID;

    public void SetInteractableID(InteractableConstants.InteractableID _ID) { InteractableID = _ID; }

    public override string GetID()
    {
        return InteractableID.ToString();
    }
}
