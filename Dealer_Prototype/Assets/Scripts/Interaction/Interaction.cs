using UnityEngine;

public interface IInteraction
{
    public abstract void MouseClick();

    public abstract void MouseEnter();

    public abstract void MouseExit();

    public abstract void ToggleOutlineShader(bool flag);

    public abstract string GetID();

    public abstract bool HasBeenInteractedWith(NPCComponent npc);

    public abstract void OnInteraction();

    public abstract Transform GetInteractionTransform();
}
