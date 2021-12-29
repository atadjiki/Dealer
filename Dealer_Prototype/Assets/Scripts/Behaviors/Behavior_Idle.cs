using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine;

public class Behavior_Idle : CharacterBehaviorScript
{

    internal override void BeginBehavior()
    {
        base.BeginBehavior();

        _data.Character.SetCurrentBehavior(CharacterConstants.BehaviorType.Idle);
    }

    protected override IEnumerator Behavior()
    {
        _data.Character.FadeToAnimation(AnimationConstants.Animations.Idle, 0.1f, false);

        yield return base.Behavior();
    }
}
