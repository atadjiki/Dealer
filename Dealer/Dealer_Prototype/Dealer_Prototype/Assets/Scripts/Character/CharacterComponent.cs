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

        GameObject Model = PrefabFactory.Instance.GetCharacterPrefab(CharacterID);
        Model.transform.parent = GetComponentInChildren<NavigatorComponent>().transform;

        _animator = GetComponentInChildren<Animator>();
        _navigator = GetComponentInChildren<NavigatorComponent>();

        _canvas = PrefabFactory.Instance.CreatePrefab(Prefab.Character_Canvas, _animator.transform);

        CurrentState = CharacterConstants.State.Idle;

        CameraManager.Instance.RegisterCharacterCamera(this);

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
}
