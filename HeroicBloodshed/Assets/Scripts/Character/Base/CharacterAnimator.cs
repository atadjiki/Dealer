using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CharacterAnimator : MonoBehaviour
{
    private Animator _animator;

    private Game.WeaponID _weaponID;

    private void Awake()
    {
        _animator = GetComponent<Animator>();    
    }

    public void Setup(Anim.State initialState, Game.WeaponID weapon)
    {
        _weaponID = weapon;

        GoTo(initialState);
       
    }

    public void GoTo(Anim.State state)
    {
        Anim.ID anim = Anim.GetAnimByWeaponType(_weaponID, state);

        _animator.CrossFade(anim.ToString(), 0.0f);

        Debug.Log("Transitioning to anim " + anim);
    }
}
