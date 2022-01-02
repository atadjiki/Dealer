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
        yield return BehaviorHelper.PerformIdle(_data);

        yield return base.Behavior();
    }
}
