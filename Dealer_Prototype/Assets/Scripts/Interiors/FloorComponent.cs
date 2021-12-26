using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorComponent : MonoBehaviour, IInterior
{
    public void MouseClick(Vector3 location)
    {
        PlayableCharacterManager.Instance.AttemptMoveOnPossesedCharacter(location);
    }

    public void MouseEnter()
    {
        Debug.Log("mouse enter floor");

        if (PlayableCharacterManager.Instance.IsCharacterCurrentlySelected())
            GameplayCanvas.Instance.SetInteractionTipTextContext(GameplayCanvas.InteractionContext.Move);
        else
            GameplayCanvas.Instance.ClearInteractionTipText();
    }

    public void MouseExit()
    {
    }
}
