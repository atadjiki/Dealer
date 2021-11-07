using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine;

public class CharacterComponent : MonoBehaviour
{
    
    public CharacterConstants.State CurrentState;

    internal Animator _animator;
    internal NavigatorComponent _navigator;

    internal float moveRadius = 30;

    public AnimationConstants.Animations DefaultAnimation = AnimationConstants.Animations.Idle;

    internal void Initialize()
    {
        
        _animator = GetComponentInChildren<Animator>();
        _navigator = GetComponentInChildren<NavigatorComponent>();

        PlayDefaultAnimation();
        CurrentState = CharacterConstants.State.Idle;

        CameraManager.Instance.RegisterCharacterCamera(this);
    }

    private void OnDestroy()
    {
        CameraManager.Instance.UnRegisterCharacterCamera(this);
    }

    public void PlayDefaultAnimation()
    {
        FadeToAnimation(AnimationConstants.GetAnimByEnum(DefaultAnimation), 0.3f);
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
}
