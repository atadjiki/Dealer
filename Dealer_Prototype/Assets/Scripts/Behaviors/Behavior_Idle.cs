using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine;

public class Behavior_Idle : CharacterBehaviorScript
{

    internal override void BeginBehavior(BehaviorData data)
    {
        base.BeginBehavior(data);

        _data.Character.SetCurrentBehavior(CharacterConstants.BehaviorType.Idle);
    }

    protected override IEnumerator Behavior()
    {
        yield return new WaitForSeconds(Random.Range(0.0f, _data.Character.IdleSeconds_Max));

        yield return base.Behavior();
    }
}
