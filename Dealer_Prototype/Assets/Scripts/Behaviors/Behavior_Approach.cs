using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine;

public class Behavior_Approach : CharacterBehaviorScript
{

    internal override void BeginBehavior()
    {
        base.BeginBehavior();

        _data.Character.SetCurrentBehavior(CharacterConstants.BehaviorType.Approach);
    }

    protected override IEnumerator Behavior()
    {
        float time_before = Time.time;

        if (_data.Character.GetNavigatorComponent().MoveToLocation(_data.Interactable.GetInteractionTransform().position))
        {
            yield return new WaitWhile(() => _data.Character.GetNavigatorComponent().State == NavigatorComponent.MovementState.Moving);

            DebugManager.Instance.Print(DebugManager.Log.LogBehavior, "Task took " + Mathf.Abs(Time.time - time_before) + " seconds");

            if (Vector3.Distance(_data.Character.GetNavigatorComponent().transform.position, _data.Interactable.GetInteractionTransform().position) < 0.1f)
            {
                _data.Character.GetNavigatorComponent().TeleportToLocation(_data.Interactable.GetInteractionTransform());
                DebugManager.Instance.Print(DebugManager.Log.LogBehavior, _data.Character.GetID() + " teleporting to  " + _data.Interactable.GetID());
            }

        }

        yield return base.Behavior();
    }
}
