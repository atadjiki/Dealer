using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine;

public class Behavior_ApproachCharacter : CharacterBehaviorScript
{
    internal override void BeginBehavior()
    {
        base.BeginBehavior();

        _data.Character.SetCurrentBehavior(AIConstants.BehaviorType.Approach_Character);
    }

    protected override IEnumerator Behavior()
    {
        yield return BehaviorHelper.AttemptStand(_data);
        yield return BehaviorHelper.PerformApproachCharacter(_data);
        yield return BehaviorHelper.ResolvePerformNPCInteraction(_data);
        yield return base.Behavior();
    }
}
