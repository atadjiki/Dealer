using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine;

public class Behavior_MoveToLocation : CharacterBehaviorScript
{
    internal override void BeginBehavior()
    {
        base.BeginBehavior();

        _data.Character.SetCurrentBehavior(CharacterConstants.BehaviorType.MoveToLocation);
    }

    protected override IEnumerator Behavior()
    {
        yield return BehaviorHelper.PerformMoveToDestination(_data);

        yield return base.Behavior();
    }
}
