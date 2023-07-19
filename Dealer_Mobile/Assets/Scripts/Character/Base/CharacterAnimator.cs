using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CharacterAnimator : MonoBehaviour
{
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();    
    }

    public void Setup(AnimationConstants.State initialState)
    {
        GoTo(initialState);
       
    }

    public void GoTo(AnimationConstants.State state)
    {
        //TODO fix setup weapon
        AnimationConstants.Anim anim = AnimationConstants.GetAnimByWeaponType(CharacterConstants.WeaponID.Pistol, state);

        _animator.CrossFade(anim.ToString(), 0.15f); ;
    }
}
