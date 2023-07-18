using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CharacterAnimator : MonoBehaviour
{
    private Animator _animator;
    private CharacterSpawnData _data;
    private CharacterConstants.TeamID _team;

    private void Awake()
    {
        _animator = GetComponent<Animator>();    
    }

    public void Setup(CharacterSpawnData data, CharacterConstants.TeamID team, AnimationConstants.State initialState)
    {
        _data = data;
        _team = team;

        GoTo(initialState);
       
    }

    public void GoTo(AnimationConstants.State state)
    {
        CharacterConstants.WeaponID weapon = CharacterConstants.GetWeapon(_data.ClassID, _team);

        AnimationConstants.Anim anim = AnimationConstants.GetAnimByWeaponType(weapon, state);

        _animator.CrossFade(anim.ToString(), 0.15f); ;
    }
}
