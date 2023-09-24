using System.Collections;
using System.Collections.Generic;
using static Constants;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CharacterAnimator : MonoBehaviour, ICharacterEventReceiver
{
    private Animator _animator;

    private WeaponID _weaponID;

    private void Awake()
    {
        _animator = GetComponent<Animator>();    
    }

    public void SwitchToRagdoll(float delay)
    {
        StartCoroutine(Coroutine_EnableRagdoll(delay));
    }

    private IEnumerator Coroutine_EnableRagdoll(float delay)
    {
        yield return new WaitForSeconds(delay);

        _animator.enabled = false;
    }

    public void Setup(AnimState initialState, WeaponID weapon)
    {
        _weaponID = weapon;

        GoTo(initialState);
       
    }

    public void GoTo(AnimState state)
    {
        GoTo(state, 0);
    }

    public void GoTo(AnimState state, float transitionTime)
    {
        AnimID anim;

        if (state == AnimState.Dead)
        {
            anim = GetUnarmedAnim(state);
        }
        else
        {
            anim = GetAnimByWeaponType(_weaponID, state);
        }

        Debug.Log("Animation: " + anim.ToString());
        _animator.CrossFade(anim.ToString(), transitionTime);
    }

    public void HandleEvent(CharacterEvent characterEvent)
    {
        switch (characterEvent)
        {
            case CharacterEvent.DEAD:
                GoTo(AnimState.Dead);
                SwitchToRagdoll(0.5f);
                break;
            default:
                break;
        }
    }
}
