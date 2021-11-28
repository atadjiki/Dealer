using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine;

public class MoveToRandomLocation : NPCBehaviorScript
{
    protected override IEnumerator Behavior()
    {
        if (_data.NPC.GetLastAction() == CharacterConstants.ActionType.Idle)
        {
            if (DebugManager.Instance.LogCharacter) Debug.Log(_data.NPC.GetID() + " - Action - MoveToRandomPoint");
            _data.NPC.PerformAction(CharacterConstants.ActionType.Move);

        }
        else if (_data.NPC.GetLastAction() == CharacterConstants.ActionType.Move || _data.NPC.GetLastAction() == CharacterConstants.ActionType.None)
        {
            if (DebugManager.Instance.LogCharacter) Debug.Log(_data.NPC.GetID() + " - Action - Idle");
            _data.NPC.PerformAction(CharacterConstants.ActionType.Idle);
        }

        yield return new WaitWhile(() => _data.NPC.GetCurrentState() == CharacterConstants.State.Moving);

        StartCoroutine(base.Behavior());
    }
}
