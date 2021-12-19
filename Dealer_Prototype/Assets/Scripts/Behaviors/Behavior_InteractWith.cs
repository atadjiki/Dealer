using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Behavior_InteractWith : CharacterBehaviorScript
{
    protected override IEnumerator Behavior()
    {
        _data.Character.SetUpdateState(Constants.CharacterConstants.UpdateState.Busy);

        //move NPC to interaction location
        _data.Character.GetNavigatorComponent().TeleportToLocation(_data.Interactable.GetInteractionTransform());
        if(DebugManager.Instance.LogBehavior) Debug.Log(_data.Character.GetID() + " teleporting to  " + _data.Interactable.GetID());

        _data.Character.ToInteracting();

        if (DebugManager.Instance.LogBehavior) Debug.Log(_data.Character.GetID() + " interacting with " + _data.Interactable.GetID());
        yield return new WaitForSeconds(3.0f);

        _data.Character.ToIdle();

        if (DebugManager.Instance.LogBehavior) Debug.Log(_data.Character.GetID() + " finished interacting with " + _data.Interactable.GetID());
        _data.Character.SetUpdateState(Constants.CharacterConstants.UpdateState.Ready);

        yield return base.Behavior();
    }
}
