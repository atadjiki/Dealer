using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCBehaviorScript : MonoBehaviour
{
    [System.Serializable]
    public struct BehaviorData
    {
        public NPCComponent NPC;//what NPC is this running on
        public Interactable Interactable;//if an interactable is involved
        public NPCBehaviorScript Behavior;
    }

    protected BehaviorData _data;

    internal Coroutine _coroutine;

    public enum BehaviorState { None, Ready, Busy, Completed };
    private BehaviorState _behaviorState = BehaviorState.None;

    public void SetBehaviorState(BehaviorState newState) { _behaviorState = newState; }

    internal virtual void BeginBehavior(BehaviorData data)
    {
        _data = data;
        SetBehaviorState(BehaviorState.Busy);
        data.NPC.SetUpdateState(Constants.CharacterConstants.UpdateState.Busy);
        _coroutine = StartCoroutine(Behavior());

        Debug.Log("Begin Behavior - " + this.name);
    }

    protected virtual IEnumerator Behavior()
    {
        EndBehavior();
        yield return null;
    }

    protected virtual void EndBehavior()
    {
        Debug.Log("End Behavior - " + this.name);
        SetBehaviorState(BehaviorState.Completed);
        _data.NPC.SetUpdateState(Constants.CharacterConstants.UpdateState.Ready);

       // Debug.Log("On behavior finished");
        _data.NPC.GoToIdle();

        Destroy(this.gameObject);
        //  OnBehaviorFinished(_data);
    }

    internal virtual void AbortBehavior()
    {
        if(_coroutine != null) StopCoroutine(_coroutine);
    }
}
