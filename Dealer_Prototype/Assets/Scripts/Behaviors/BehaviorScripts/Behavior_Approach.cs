using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine;

public class Behavior_Approach : CharacterBehaviorScript
{

    internal override void BeginBehavior()
    {
        base.BeginBehavior();

        _data.Character.SetCurrentBehavior(AIConstants.BehaviorType.Approach);
    }

    protected override IEnumerator Behavior()
    {
        yield return BehaviorHelper.PerformApproachInteractable(_data);
        yield return BehaviorHelper.ResolvePerformInteraction(_data);
        yield return base.Behavior();
    }
}
