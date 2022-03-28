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
            return InteractableConstants.InteractionContext.Move;
        }
        else
        {
            return InteractableConstants.InteractionContext.None;
        }
    }

    public void MouseExit()
    {
    }
}
