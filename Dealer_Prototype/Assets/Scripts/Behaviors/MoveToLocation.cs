using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine;

public class MoveToLocation : CharacterBehaviorScript
{
    protected override IEnumerator Behavior()
    {
        _data.Character.GetNavigatorComponent().MoveToLocation(_data.Destination);

        yield return new WaitWhile(() => _data.Character.GetCurrentState() == CharacterConstants.State.Moving);

        StartCoroutine(base.Behavior());
    }
}
