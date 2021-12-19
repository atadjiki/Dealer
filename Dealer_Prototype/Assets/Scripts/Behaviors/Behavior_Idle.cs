using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine;

public class Behavior_Idle : CharacterBehaviorScript
{
    protected override IEnumerator Behavior()
    {
        _data.Character.PreviousBehavior = CharacterConstants.BehaviorType.Idle;

        yield return new WaitForSeconds(Random.Range(0.0f, _data.Character.IdleSeconds_Max));

        yield return base.Behavior();
    }
}
