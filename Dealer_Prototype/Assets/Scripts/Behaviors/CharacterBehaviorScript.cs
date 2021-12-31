using System.Collections;
using System.Collections.Generic;
using Constants;
using TMPro;
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
    public BehaviorState GetBehaviorState() { return _behaviorState; }

    private GameObject behaviorDecal;

    internal virtual void Setup(BehaviorData data)
    {
        _data = data;
        SetBehaviorState(BehaviorState.Ready);

        behaviorDecal = PrefabFactory.CreatePrefab(RegistryID.BehaviorDecal, _data.Destination, Quaternion.identity, null);

        behaviorDecal.transform.parent = this.gameObject.transform;

        ColorManager.Instance.SetObjectToColor(behaviorDecal, ColorManager.Instance.GetBehaviorDecalColor(false));
    }

    internal virtual void BeginBehavior()
    {
        DebugManager.Instance.Print(DebugManager.Log.LogBehavior, "Begin Behavior - " + this.name);
        SetBehaviorState(BehaviorState.Busy);
        _data.Character.SetUpdateState(Constants.CharacterConstants.UpdateState.Busy);
        _coroutine = StartCoroutine(Behavior());

        ColorManager.Instance.SetObjectToColor(behaviorDecal, ColorManager.Instance.GetBehaviorDecalColor(true));
    }

    protected virtual IEnumerator Behavior()
    {
        EndBehavior();
        yield return null;
    }

    protected virtual void EndBehavior()
    {
        DebugManager.Instance.Print(DebugManager.Log.LogBehavior, "End Behavior - " + this.name);
        SetBehaviorState(BehaviorState.Completed);
        _data.Character.SetUpdateState(Constants.CharacterConstants.UpdateState.Ready);
        _data.Character.SetCurrentBehavior(Constants.CharacterConstants.BehaviorType.None);

        _data.Character.OnBehaviorFinished(this);

        Destroy(behaviorDecal.gameObject);
        Destroy(this.gameObject);
    }

    internal virtual void AbortBehavior()
    {
        DebugManager.Instance.Print(DebugManager.Log.LogBehavior, "Aborting script " + gameObject.name);
        if (_coroutine != null) StopCoroutine(_coroutine);

        SetBehaviorState(BehaviorState.Completed);

        Destroy(behaviorDecal.gameObject);
        Destroy(this.gameObject);

    }
}
