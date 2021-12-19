using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractWithJukebox : NPCBehaviorScript
{
    protected override IEnumerator Behavior()
    {
        _data.NPC.SetUpdateState(Constants.CharacterConstants.UpdateState.Busy);

        //move NPC to interaction location
        _data.NPC.MoveToLocation(_data.Interactable.GetInteractionTransform().position);
         Debug.Log(_data.NPC.GetID() + " moving to  " + _data.Interactable.GetID());

        yield return new WaitUntil(() => _data.NPC.GetCurrentState() != Constants.CharacterConstants.State.Moving);

        //rotate NPC to interaction location
        _data.NPC.GoToIdle();
        Debug.Log(_data.NPC.GetID() + " idling at  " + _data.Interactable.GetID());
        yield return new WaitForSeconds(0.5f);

        _data.NPC.ToInteracting();

        Debug.Log(_data.NPC.GetID() + " interacting with " + _data.Interactable.GetID());
        yield return new WaitForSeconds(3.0f);

        _data.NPC.GoToIdle();

        Debug.Log(_data.NPC.GetID() + " finished interacting with " + _data.Interactable.GetID());
        _data.NPC.SetUpdateState(Constants.CharacterConstants.UpdateState.Ready);

        StartCoroutine(base.Behavior());
    }
}
