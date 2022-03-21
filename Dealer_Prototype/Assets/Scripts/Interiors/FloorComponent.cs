using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine;

public class FloorComponent : MonoBehaviour, IInterior
{
    public void MouseClick(Vector3 location)
    {
        PlayableCharacterManager.Instance.AttemptMoveOnPossesedCharacter(location);
    }

    public void MouseEnter()
    {
        if (PlayableCharacterManager.Instance.IsCharacterCurrentlySelected())
            UIManager.Instance.HandleEvent(InteractableConstants.InteractionContext.Move);
        else
            UIManager.Instance.HandleEvent(UI.Events.Clear);

        CursorManager.Instance.ToDefault();
    }

    public void MouseExit()
    {
    }
}
