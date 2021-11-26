using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine.UI;
using UnityEngine;

public class CharacterComponent : MonoBehaviour
{
    internal Animator _animator;
    internal NavigatorComponent _navigator;
    internal CharacterCanvas _charCanvas;
    internal InteractionComponent _interaction;

    [Header("Character ID")]
    [SerializeField] internal CharacterConstants.CharacterID CharacterID;

    [Header("Character Setup")]

    [SerializeField] internal CharacterConstants.Behavior BehaviorMode = CharacterConstants.Behavior.Wander;

    [Header("Debug")]

    [SerializeField] private CharacterConstants.UpdateState updateState = CharacterConstants.UpdateState.None;
    [SerializeField] private CharacterConstants.State CurrentState;

    internal CharacterConstants.ActionType LastAction = CharacterConstants.ActionType.None;
    internal Coroutine ActionCoroutine;

    internal float moveRadius = 30;

    internal void Initialize()
    {
        StartCoroutine(DoInitialize());
    }

    internal IEnumerator DoInitialize()
    {
        //setup navigator
        GameObject NavigtorPrefab = PrefabFactory.Instance.CreatePrefab(RegistryID.Navigator, this.transform);
        _navigator = NavigtorPrefab.GetComponent<NavigatorComponent>();

        yield return new WaitWhile(() => _navigator == null);

        //

        //setup character model and attach to navigator
        GameObject ModelPrefab = PrefabFactory.Instance.GetCharacterPrefab(CharacterID);
        ModelPrefab.transform.parent = NavigtorPrefab.transform;
        _animator = ModelPrefab.GetComponent<Animator>();

        yield return new WaitWhile(() => _animator == null);

        //

        //attach a UI canvas to the model 
        GameObject CanvasPrefab = PrefabFactory.Instance.CreatePrefab(RegistryID.CharacterCanvas, ModelPrefab.transform);
        _charCanvas = CanvasPrefab.GetComponent<CharacterCanvas>();

        yield return new WaitWhile(() => _charCanvas == null);

        _charCanvas.Set_Text_ID(this.CharacterID.ToString());

        ///

        GameObject InteractionPrefab = PrefabFactory.Instance.CreatePrefab(RegistryID.Interaction, NavigtorPrefab.transform);
        _interaction = InteractionPrefab.GetComponent<InteractionComponent>();

        yield return new WaitWhile(() => _interaction == null);

        _interaction.MouseEnterEvent += OnMouseEnter;
        _interaction.MouseExitEvent += OnMouseExit;

        //idle to tart with 
        SetCurrentState(CharacterConstants.State.Idle);

        //register camera
        CameraManager.Instance.RegisterCharacterCamera(this);

        //ready to begin behaviors
        updateState = CharacterConstants.UpdateState.Ready; //let the manager know we're ready to be handled

       

        yield return null;
    }

    public void SetPositionRotation(Vector3 Position, Quaternion Rotation)
    {
        _navigator.gameObject.transform.position = Position;
        _navigator.gameObject.transform.rotation = Rotation;

    }
    public void OnMouseEnter()
    {
        _charCanvas.Toggle(true);

    }

    public void OnMouseExit()
    {
        _charCanvas.Toggle(false);
    }

    private void OnDestroy()
    {
        CameraManager.Instance.UnRegisterCharacterCamera(this);
    }

    public virtual void OnNewDestination(Vector3 destination) { }
    public virtual void OnDestinationReached(Vector3 destination) { }

    public void ToIdle()
    {
        SetCurrentState(CharacterConstants.State.Idle);
        _animator.CrossFade(AnimationConstants.Idle, 0.5f);
        _navigator.SetCanMove(false);
    }

    public void ToMoving()
    {
        SetCurrentState(CharacterConstants.State.Moving);
        _animator.CrossFade(AnimationConstants.Walking, 0.1f);
        _navigator.SetCanMove(true);
    }

    public void ToInteracting()
    {
        StopAllCoroutines();
        SetCurrentState(CharacterConstants.State.Interacting);
        _animator.CrossFade(AnimationConstants.ButtonPush, 0.3f);
        _navigator.SetCanMove(false);

    }

    public void ToTalking()
    {
        StopAllCoroutines();
        SetCurrentState(CharacterConstants.State.Talking);
        _animator.CrossFade(AnimationConstants.Talking, 0.3f);
        _navigator.SetCanMove(false);
    }

    public void ToSitting()
    {
        StopAllCoroutines();
        SetCurrentState(CharacterConstants.State.Sitting);
        FadeToAnimation(AnimationConstants.Male_Sitting_2, 0.35f);
        _navigator.SetCanMove(false);
    }

   

    private void FadeToAnimation(string animation, float time)
    {
        _animator.CrossFade(animation, time);
    }

    internal void SetCurrentState(CharacterConstants.State newState)
    {
        CurrentState = newState;
        _charCanvas.Set_Text_State(CurrentState.ToString());
    }

    public CharacterConstants.State GetCurrentState() { return CurrentState; }

    internal void SetUpdateState(CharacterConstants.UpdateState newState)
    {
        updateState = newState;
    }

    public CharacterConstants.UpdateState GetUpdateState() { return updateState; }

    public CharacterConstants.ActionType GetLastAction() { return LastAction; }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if(updateState == CharacterConstants.UpdateState.None)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(this.transform.position, 0.5f);
            Gizmos.DrawIcon(this.transform.position + new Vector3(0,0,0.5f), "Icon_Person_Male.png"); ;
        }
    }
#endif
}
