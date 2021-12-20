using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine;

public class Behavior_InteractWith : CharacterBehaviorScript
{
    internal override void BeginBehavior()
    {
        base.BeginBehavior();

        _data.Character.SetCurrentBehavior(CharacterConstants.BehaviorType.Interact);
    }

    protected override IEnumerator Behavior()
    {
        _data.Character.SetUpdateState(CharacterConstants.UpdateState.Busy);

        //move NPC to interaction location
        _data.Character.GetNavigatorComponent().TeleportToLocation(_data.Interactable.GetInteractionTransform());
        if(DebugManager.Instance.LogBehavior) Debug.Log(_data.Character.GetID() + " teleporting to  " + _data.Interactable.GetID());

        _data.Character.ToInteracting();

        if (DebugManager.Instance.LogBehavior) Debug.Log(_data.Character.GetID() + " interacting with " + _data.Interactable.GetID());
        yield return new WaitForSeconds(3.0f);

        _data.Character.ToIdle();

        if (DebugManager.Instance.LogBehavior) Debug.Log(_data.Character.GetID() + " finished interacting with " + _data.Interactable.GetID());
        _data.Character.SetUpdateState(CharacterConstants.UpdateState.Ready);

        yield return base.Behavior();
    }
}
