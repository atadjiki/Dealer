using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CharacterAnimator : MonoBehaviour
{
    private Animator _animator;
    private CharacterData _data;

    private void Awake()
    {
        _animator = GetComponent<Animator>();    
    }

    public void Setup(CharacterData data, AnimationConstants.State initialState)
    {
        _data = data;

        GoTo(initialState);
       
    }

    public void GoTo(AnimationConstants.State state)
    {
        AnimationConstants.Anim anim = AnimationConstants.GetAnimByWeaponType(_data.weapon, state);

        _animator.CrossFade(anim.ToString(), 0.15f); ;
    }
}
