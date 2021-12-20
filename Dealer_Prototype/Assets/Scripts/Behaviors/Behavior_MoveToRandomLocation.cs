using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine;

public class Behavior_MoveToRandomLocation : CharacterBehaviorScript
{
    internal override void BeginBehavior()
    {
        base.BeginBehavior();

        _data.Character.SetCurrentBehavior(CharacterConstants.BehaviorType.MoveToRandomLocation);
    }

    protected override IEnumerator Behavior()
    {
        _data.Character.GetNavigatorComponent().MoveToRandomLocation();
        
        yield return new WaitWhile(() => _data.Character.GetNavigatorComponent().State == NavigatorComponent.MovementState.Moving);

        yield return base.Behavior();
    }
}
