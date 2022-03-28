using UnityEngine;
using Constants;

public interface IInterior
{
    public abstract void MouseClick(Vector3 location);

    public abstract InteractableConstants.InteractionContext MouseEnter();

    public abstract void MouseExit();
}
