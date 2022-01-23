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
            GameplayCanvas.Instance.SetInteractionTipTextContext(InteractableConstants.InteractionContext.Move);
        else
            GameplayCanvas.Instance.ClearInteractionTipText();

        CursorManager.Instance.ToDefault();
    }

    public void MouseExit()
    {
    }
}
