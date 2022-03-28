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

    public InteractableConstants.InteractionContext MouseEnter()
    {
        CursorManager.Instance.ToDefault();

        if (PlayableCharacterManager.Instance.IsCharacterCurrentlySelected())
        {
            UIManager.Instance.HandleEvent(InteractableConstants.InteractionContext.Move);
            return InteractableConstants.InteractionContext.Move;
        }
        else
        {
            UIManager.Instance.HandleEvent(UI.Events.Clear);
            return InteractableConstants.InteractionContext.None;
        }
    }

    public void MouseExit()
    {
    }
}
