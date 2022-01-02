using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine;

public class Behavior_InteractWith : CharacterBehaviorScript
{
    internal override void BeginBehavior()
    {
        base.BeginBehavior();

        _data.Character.SetCurrentBehavior(CharacterConstants.BehaviorType.Interact);
    }

    protected override IEnumerator Behavior()
    {
        yield return BehaviorHelper.PerformInteractWith(_data);

        yield return base.Behavior();
    }
}
