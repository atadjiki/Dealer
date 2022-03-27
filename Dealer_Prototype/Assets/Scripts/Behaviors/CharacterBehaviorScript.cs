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
        public CharacterComponent Interactee;
        public IInteraction Interactable;//if an interactable is involved
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

    private GameObject objectNote_obj;
    private ObjectNote objectNote;

    private void Awake()
    {
        objectNote_obj = new GameObject();
        objectNote_obj.transform.parent = this.transform;
        objectNote = objectNote_obj.AddComponent<ObjectNote>();
        objectNote.ShowInGameEditor = true;
        objectNote.NoteText = "";
    }

    private void FixedUpdate()
    {
        if(DebugManager.Instance.State_Behavior == DebugManager.State.LogAndVisual || DebugManager.Instance.State_Behavior == DebugManager.State.VisualLogOnly)
        {
            objectNote.NoteText =
                this.gameObject.name + "\n" +
                "State: " + this.GetBehaviorState().ToString() + "\n";

            if(GetBehaviorState() == BehaviorState.Ready)
            {
                objectNote.Color = Color.grey;
            }
            else if(GetBehaviorState() == BehaviorState.Busy)
            {
                objectNote.Color = Color.green;
            }
            else if(GetBehaviorState() == BehaviorState.Completed)
            {
                objectNote.Color = Color.red;
            }

            objectNote.Color.a = 0.05f;
        }
        else
        {
            objectNote.NoteText = "";
        }
    }

    internal virtual void Setup(BehaviorData data)
    {
        _data = data;
        SetBehaviorState(BehaviorState.Ready);

        behaviorDecal = PrefabFactory.CreatePrefab(RegistryID.BehaviorDecal, _data.Destination, Quaternion.identity, null);
        objectNote_obj.transform.position = _data.Destination;

        behaviorDecal.transform.parent = this.gameObject.transform;

        ColorManager.Instance.SetObjectToColor(behaviorDecal, ColorManager.Instance.GetBehaviorDecalColor(false));
    }

    internal virtual void BeginBehavior()
    {
        DebugManager.Instance.Print(DebugManager.Log.LogBehavior, "Begin Behavior - " + this.name);
        SetBehaviorState(BehaviorState.Busy);
        _data.Character.SetUpdateState(AIConstants.UpdateState.Busy);
        _coroutine = StartCoroutine(Behavior());

        ColorManager.Instance.SetObjectToColor(behaviorDecal, ColorManager.Instance.GetBehaviorDecalColor(true));
    }

    protected virtual IEnumerator Behavior()
    {
        DebugManager.Instance.Print(DebugManager.Log.LogBehavior, "Running Behavior - " + this.name);
        EndBehavior();
        yield return null;
    }

    protected virtual void EndBehavior()
    {
        DebugManager.Instance.Print(DebugManager.Log.LogBehavior, "End Behavior - " + this.name);
        SetBehaviorState(BehaviorState.Completed);
        _data.Character.SetUpdateState(AIConstants.UpdateState.Ready);
        _data.Character.SetCurrentBehavior(AIConstants.BehaviorType.None);

        _data.Character.OnBehaviorFinished(this);

        StartCoroutine(DoDestroy());
    }

    internal virtual void AbortBehavior()
    {
        DebugManager.Instance.Print(DebugManager.Log.LogBehavior, "Aborting script " + gameObject.name);
        if (_coroutine != null) StopCoroutine(_coroutine);

        SetBehaviorState(BehaviorState.Completed);

        Destroy(behaviorDecal.gameObject);
      //  Destroy(this.gameObject);

    }

    private IEnumerator DoDestroy()
    {
        Destroy(behaviorDecal);
        yield return new WaitForSeconds(1.0f);
        Destroy(this.gameObject);
    }
}
