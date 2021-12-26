using UnityEngine;

public interface IInterior
{
    public abstract void MouseClick(Vector3 location);

    public abstract void MouseEnter();

    public abstract void MouseExit();
}
