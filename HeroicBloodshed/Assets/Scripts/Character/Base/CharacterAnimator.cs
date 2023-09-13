using System.Collections;
using System.Collections.Generic;
using static Constants;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CharacterAnimator : MonoBehaviour
{
    private Animator _animator;

    private WeaponID _weaponID;

    private void Awake()
    {
        _animator = GetComponent<Animator>();    
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

        _animator.CrossFade(anim.ToString(), transitionTime);
    }
}
