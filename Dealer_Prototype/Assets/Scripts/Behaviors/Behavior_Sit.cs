using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine;

public class Behavior_Sit : CharacterBehaviorScript
{
    internal override void BeginBehavior()
    {
        base.BeginBehavior();

        _data.Character.SetCurrentBehavior(CharacterConstants.BehaviorType.Sit);
    }

    protected override IEnumerator Behavior()
    {
        _data.Character.SetUpdateState(CharacterConstants.UpdateState.Busy);

        Chair chair = (Chair)_data.Interactable;

        if(chair != null)
        {
            Transform sittingTransform = chair.GetSittingPoseTransform();

            //move NPC to interaction location
            _data.Character.GetNavigatorComponent().transform.position = sittingTransform.position;
            _data.Character.GetNavigatorComponent().transform.rotation = sittingTransform.rotation;

            DebugManager.Instance.Print(DebugManager.Log.LogBehavior, _data.Character.GetID() + " teleporting to  " + _data.Interactable.GetID());

            _data.Character.FadeToAnimation(AnimationConstants.Animations.Sitting_Idle, 0.05f, false);

            DebugManager.Instance.Print(DebugManager.Log.LogBehavior, _data.Character.GetID() + " sitting on " + _data.Interactable.GetID());

            _data.Character.SetUpdateState(CharacterConstants.UpdateState.Ready);
        }

        yield return base.Behavior();
    }
}
