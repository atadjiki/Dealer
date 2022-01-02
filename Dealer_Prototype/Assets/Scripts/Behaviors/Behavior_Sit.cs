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
        yield return BehaviorHelper.PerformSit(_data);

        yield return base.Behavior();
    }
}
