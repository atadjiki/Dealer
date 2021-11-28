using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jukebox : Interactable
{

    public override void OnMouseEnter()
    {
        base.OnMouseEnter();
    }

    public override void OnMouseExit()
    {
        base.OnMouseExit();
    }

    public override void OnMouseClicked()
    {
        base.OnMouseClicked();

        Debug.Log("Clicked on " + GetID());
    }

    internal override void Interaction(NPCComponent interactee)
    {
        _interactee = interactee;
        _interactedWith.Add(interactee);
        base.Interaction(interactee); 
    }

    internal override IEnumerator DoInteraction()
    {
        _interactee.SetUpdateState(Constants.CharacterConstants.UpdateState.Busy);

        //move NPC to interaction location
        _interactee.MoveToLocation(InteractionTransform.localPosition);
        Debug.Log(_interactee.GetID() + " moving to  " + this.GetID());

        yield return new WaitUntil(() => _interactee.GetCurrentState() != Constants.CharacterConstants.State.Moving);

        //rotate NPC to interaction location
        _interactee.ToIdle();
        Debug.Log(_interactee.GetID() + " idling at  " + this.GetID());
        yield return new WaitForSeconds(0.5f);

        _interactee.ToInteracting();

        Debug.Log(_interactee.GetID() + " interacting with " + this.GetID());
        yield return new WaitForSeconds(3.0f);

        _interactee.ToIdle();

        Debug.Log(_interactee.GetID() + " finished interacting with " + this.GetID());
        _interactee.SetUpdateState(Constants.CharacterConstants.UpdateState.Ready);

        _interactee = null;

        base.DoInteraction();

    }
}
