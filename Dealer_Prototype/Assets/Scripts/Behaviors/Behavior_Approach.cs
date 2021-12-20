using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine;

public class Behavior_Approach : CharacterBehaviorScript
{

    internal override void BeginBehavior(BehaviorData data)
    {
        base.BeginBehavior(data);

        data.Character.SetCurrentBehavior(CharacterConstants.BehaviorType.Approach);
    }

    protected override IEnumerator Behavior()
    {
        _data.Character.GetNavigatorComponent().MoveToLocation(_data.Interactable.GetInteractionTransform().position);
        if (DebugManager.Instance.LogBehavior) Debug.Log(_data.Character.GetID() + " approaching " + _data.Interactable.GetID());

        yield return new WaitWhile(() => _data.Character.GetNavigatorComponent().State == NavigatorComponent.MovementState.Moving);

        _data.Character.GetNavigatorComponent().TeleportToLocation(_data.Interactable.GetInteractionTransform());
        if (DebugManager.Instance.LogBehavior) Debug.Log(_data.Character.GetID() + " teleporting to  " + _data.Interactable.GetID());

        yield return base.Behavior();
    }
}
