using Constants;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimationComponent : MonoBehaviour
{
    private NavigatorComponent _navigator;
    private Animator _animator;

    private GameObject socket = null;
    private AnimationConstants.Anim CurrentAnimation = AnimationConstants.Anim.Idle;

    private CharacterComponent charPtr;

    private bool hidden = false;

    public void SetSocket(GameObject inObject) { socket = inObject; }

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _navigator = GetComponentInParent<NavigatorComponent>();

        charPtr = this.transform.parent.GetComponentInParent<CharacterComponent>();

        _navigator.SetCanMove(true);

        SetSocket(this.transform.parent.gameObject);
    }

    public void FadeToAnimation(AnimationConstants.Anim anim, float time, bool canMove)
    {
        string animString = AnimationConstants.FetchAnimString(charPtr.GetCharacterID(), anim);

        if (_animator != null) _animator.CrossFade(animString, time);
        if (_navigator != null) _navigator.SetCanMove(canMove);
        SetCurrentAnimation(anim);

        DebugManager.Instance.Print(DebugManager.Log.LogCharacter, "Fading to anim " + animString);
    }

    public AnimationConstants.Anim GetCurrentAnimation() { return CurrentAnimation; }

    public void SetCurrentAnimation(AnimationConstants.Anim anim)
    {
        CurrentAnimation = anim;
    }

    public bool IsHidden()
    {
        return hidden;
    }

    public void ToggleVisiblity(bool flag)
    {
        hidden = flag;
        this.gameObject.SetActive(hidden);
    }

}
