using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

[RequireComponent(typeof(Animator))]
public class CharacterAnimator : MonoBehaviour
{
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void SetAnim(CharacterAnim anim, float transitionTime = 0.25f)
    {
        string state = GetAnimString(anim);

        _animator.CrossFadeInFixedTime(state, transitionTime);
    }
   
}
