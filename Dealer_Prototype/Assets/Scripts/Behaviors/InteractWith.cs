using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractWith : CharacterBehaviorScript
{
    protected override IEnumerator Behavior()
    {
        _data.Character.SetUpdateState(Constants.CharacterConstants.UpdateState.Busy);

        //move NPC to interaction location
        _data.Character.GetNavigatorComponent().MoveToLocation(_data.Interactable.GetInteractionTransform().position);
        if(DebugManager.Instance.LogBehavior) Debug.Log(_data.Character.GetID() + " moving to  " + _data.Interactable.GetID());

        yield return new WaitWhile(() => _data.Character.GetNavigatorComponent().State == NavigatorComponent.MovementState.Moving);

        //rotate NPC to interaction location
        _data.Character.ToIdle();
        if (DebugManager.Instance.LogBehavior) Debug.Log(_data.Character.GetID() + " idling at  " + _data.Interactable.GetID());
        yield return new WaitForSeconds(0.5f);

        _data.Character.ToInteracting();

        if (DebugManager.Instance.LogBehavior) Debug.Log(_data.Character.GetID() + " interacting with " + _data.Interactable.GetID());
        yield return new WaitForSeconds(3.0f);

        _data.Character.ToIdle();

        if (DebugManager.Instance.LogBehavior) Debug.Log(_data.Character.GetID() + " finished interacting with " + _data.Interactable.GetID());
        _data.Character.SetUpdateState(Constants.CharacterConstants.UpdateState.Ready);

        StartCoroutine(base.Behavior());
    }
}
