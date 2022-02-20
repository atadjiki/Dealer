using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine;

public class Behavior_MoveToRandomLocation : CharacterBehaviorScript
{
    internal override void BeginBehavior()
    {
        base.BeginBehavior();

        _data.Character.SetCurrentBehavior(AIConstants.BehaviorType.MoveToRandomLocation);
    }

    protected override IEnumerator Behavior()
    {
        yield return BehaviorHelper.PerformMoveToRandomLocation(_data);
        yield return base.Behavior();
    }
}
