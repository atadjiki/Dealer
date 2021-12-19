using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBehaviorScript : MonoBehaviour
{
    [System.Serializable]
    public struct BehaviorData
    {
        public CharacterComponent Character;//what NPC is this running on
        public Interactable Interactable;//if an interactable is involved
        public CharacterBehaviorScript Behavior;
        public Vector3 Destination;
    }

    protected BehaviorData _data;

    internal Coroutine _coroutine;

    public enum BehaviorState { None, Ready, Busy, Completed };
    private BehaviorState _behaviorState = BehaviorState.None;

    public void SetBehaviorState(BehaviorState newState) { _behaviorState = newState; }

    internal virtual void BeginBehavior(BehaviorData data)
    {
        _data = data;

        if (_data.Character != null)
        {
            _data.Character.ClearBehaviors();
        }

        SetBehaviorState(BehaviorState.Busy);
        data.Character.SetUpdateState(Constants.CharacterConstants.UpdateState.Busy);
        _coroutine = StartCoroutine(Behavior());

        if (DebugManager.Instance.LogBehavior) Debug.Log("Begin Behavior - " + this.name);
    }

    protected virtual IEnumerator Behavior()
    {
        EndBehavior();
        yield return null;
    }

    protected virtual void EndBehavior()
    {
        if (DebugManager.Instance.LogBehavior) Debug.Log("End Behavior - " + this.name);
        SetBehaviorState(BehaviorState.Completed);
        _data.Character.SetUpdateState(Constants.CharacterConstants.UpdateState.Ready);

        _data.Character.GoToIdle();

        Destroy(this.gameObject);
        //  OnBehaviorFinished(_data);
    }

    internal virtual void AbortBehavior()
    {
        if(_coroutine != null) StopCoroutine(_coroutine);
        if(DebugManager.Instance.LogBehavior) Debug.Log("Aborting script " + gameObject.name);
    }
}
