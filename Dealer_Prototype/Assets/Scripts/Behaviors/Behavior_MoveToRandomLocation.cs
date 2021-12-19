using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine;

public class Behavior_MoveToRandomLocation : CharacterBehaviorScript
{
    protected override IEnumerator Behavior()
    {
        _data.Character.PreviousBehavior = CharacterConstants.BehaviorType.MoveToLocation;

        _data.Character.GetNavigatorComponent().MoveToRandomLocation();

        yield return new WaitWhile(() => _data.Character.GetNavigatorComponent().State == NavigatorComponent.MovementState.Moving);

        yield return base.Behavior();
    }
}
