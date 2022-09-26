using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Animator))]
public class CharacterModel : MonoBehaviour
{
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();

    }
    public void SetAnimationSet(CharacterInfo info)
    {
        _animator.runtimeAnimatorController = AnimationManager.Instance.GetControllerByWeaponID(info.WeaponID);
    }

    private void OnMouseDown()
    {
        Debug.Log("mouse clicked " + this.name);
    }
}
