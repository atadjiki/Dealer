using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine;

public class Behavior_Idle : CharacterBehaviorScript
{

    internal override void BeginBehavior()
    {
        base.BeginBehavior();

        _data.Character.SetCurrentBehavior(CharacterConstants.BehaviorType.Idle);
    }

    protected override IEnumerator Behavior()
    {
        _data.Character.ToIdle();

        yield return new WaitForSeconds(Random.Range(0.0f, _data.Character.IdleSeconds_Max));

        yield return base.Behavior();
    }
}
