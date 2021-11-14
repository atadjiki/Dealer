using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine;

public class CharacterComponent : MonoBehaviour
{
    internal Animator _animator;
    internal NavigatorComponent _navigator;
    internal GameObject _canvas;

    [Header("Character ID")]
    [SerializeField] internal CharacterConstants.Characters CharacterID;

    [Header("Character Setup")]

    [SerializeField] internal CharacterConstants.Behavior BehaviorMode = CharacterConstants.Behavior.Wander;

    [Header("Debug")]

    [SerializeField] internal CharacterConstants.UpdateState updateState = CharacterConstants.UpdateState.None;
    [SerializeField] internal CharacterConstants.State CurrentState;

    internal CharacterConstants.ActionType LastAction = CharacterConstants.ActionType.None;
    internal Coroutine ActionCoroutine;
    internal float moveRadius = 30;

    internal void Initialize()
    {
        //setup navigator
        GameObject navigatorPrefab = PrefabFactory.Instance.CreatePrefab(Prefab.Navigator, this.transform);
        _navigator = navigatorPrefab.GetComponent<NavigatorComponent>();

        //setup character model and attach to navigator
        GameObject Model = PrefabFactory.Instance.GetCharacterPrefab(CharacterID);
        Model.transform.parent = navigatorPrefab.transform;
        _animator = Model.GetComponent<Animator>();

        //attach a UI canvas to the model 
        _canvas = PrefabFactory.Instance.CreatePrefab(Prefab.Character_Canvas, Model.transform);

        //idle to tart with 
        CurrentState = CharacterConstants.State.Idle;

        //register camera
        CameraManager.Instance.RegisterCharacterCamera(this);

        //ready to begin behaviors
        updateState = CharacterConstants.UpdateState.Ready; //let the manager know we're ready to be handled
    }

    private void OnDestroy()
    {
        CameraManager.Instance.UnRegisterCharacterCamera(this);
    }

    public virtual void OnNewDestination(Vector3 destination) { }
    public virtual void OnDestinationReached(Vector3 destination) { }

    public void ToIdle()
    {
        CurrentState = CharacterConstants.State.Idle;
        _animator.CrossFade(AnimationConstants.Idle, 0.5f);
        _navigator.SetCanMove(false);
    }

    public void ToMoving()
    {
        CurrentState = CharacterConstants.State.Moving;
        _animator.CrossFade(AnimationConstants.Walking, 0.1f);
        _navigator.SetCanMove(true);
    }

    public void ToInteracting()
    {
        StopAllCoroutines();
        CurrentState = CharacterConstants.State.Interacting;
        _animator.CrossFade(AnimationConstants.ButtonPush, 0.3f);
        _navigator.SetCanMove(false);

    }

    public void ToTalking()
    {
        StopAllCoroutines();
        CurrentState = CharacterConstants.State.Talking;
        _animator.CrossFade(AnimationConstants.Talking, 0.3f);
        _navigator.SetCanMove(false);
    }

    public void ToSitting()
    {
        StopAllCoroutines();
        CurrentState = CharacterConstants.State.Sitting;
        FadeToAnimation(AnimationConstants.Male_Sitting_2, 0.35f);
        _navigator.SetCanMove(false);
    }

   

    public void FadeToAnimation(string animation, float time)
    {
        _animator.CrossFade(animation, time);
    }


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
