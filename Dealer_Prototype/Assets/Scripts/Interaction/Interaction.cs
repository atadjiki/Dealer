using UnityEngine;

public struct InteractionUpdateResult
{
    public bool success;
    public Constants.InteractableConstants.InteractionContext context;
    public IInteraction interactable;

    public static InteractionUpdateResult Build()
    {
        InteractionUpdateResult result;
        result.success = false;
        result.context = Constants.InteractableConstants.InteractionContext.None;
        result.interactable = null;
        return result;
    }
    
}

public interface IInteraction
{
    public abstract void MouseClick();

    public abstract void ToggleOutlineShader(bool flag);

    public abstract string GetID();

    public abstract bool HasBeenInteractedWith(NPCComponent npc);

    public abstract void OnInteraction();

    public abstract Transform GetInteractionTransform();

    public abstract bool IsVisible();

    public abstract GameObject GetGameObject();

    public abstract Constants.InteractableConstants.InteractionContext GetContext();
}
