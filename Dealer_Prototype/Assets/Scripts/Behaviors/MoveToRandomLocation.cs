using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine;

public class MoveToRandomLocation : CharacterBehaviorScript
{
    protected override IEnumerator Behavior()
    {
        if (_data.Character.GetLastAction() == CharacterConstants.ActionType.Idle)
        {
            if (DebugManager.Instance.LogCharacter) Debug.Log(_data.Character.GetID() + " - Action - MoveToRandomPoint");
            _data.Character.PerformAction(CharacterConstants.ActionType.Move);

        }
        else if (_data.Character.GetLastAction() == CharacterConstants.ActionType.Move || _data.Character.GetLastAction() == CharacterConstants.ActionType.None)
        {
            if (DebugManager.Instance.LogCharacter) Debug.Log(_data.Character.GetID() + " - Action - Idle");
            _data.Character.PerformAction(CharacterConstants.ActionType.Idle);
        }

        yield return new WaitWhile(() => _data.Character.GetCurrentState() == CharacterConstants.State.Moving);

        StartCoroutine(base.Behavior());
    }
}
