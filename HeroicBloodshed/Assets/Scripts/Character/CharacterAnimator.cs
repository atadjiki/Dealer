using System.Collections;
using System.Collections.Generic;
using static Constants;
using UnityEngine;
using RootMotion.FinalIK;

[RequireComponent(typeof(Animator))]
public class CharacterAnimator : MonoBehaviour, ICharacterEventReceiver
{
    private Animator _animator;
    private AimIK _aimIK;

    private WeaponID _weaponID;

    private bool _canReceive = true;

    private Rigidbody[] _rigidbodies;

    private void Awake()
    {
        _animator = GetComponent<Animator>();

        _aimIK = GetComponentInChildren<AimIK>();

        _rigidbodies = GetComponentsInChildren<Rigidbody>();
    }

    public void SwitchToRagdoll(float delay)
    {
        StartCoroutine(Coroutine_SwitchToRagdoll(delay));
    }

    private IEnumerator Coroutine_SwitchToRagdoll(float delay)
    {
        yield return new WaitForSeconds(delay);

        _animator.enabled = false;

        foreach (Collider collider in GetComponentsInChildren<Collider>())
        {
            if (!collider.isTrigger)
            {
                collider.enabled = true;
            }
        }

        foreach (Rigidbody rigidbody in _rigidbodies)
        {
            rigidbody.isKinematic = false;
        }
    }

    public void Setup(AnimID initialState, WeaponID weapon, Transform offset)
    {
        _weaponID = weapon;

        int layerIndex = GetLayerByWeapon(_weaponID);

        _animator.SetLayerWeight(layerIndex, 1);

        _aimIK.solver.transform = offset;

        Rigidbody[] rigidbodies = GetComponentsInChildren<Rigidbody>();

        foreach (Rigidbody rigidbody in rigidbodies)
        {
            if(rigidbody.gameObject.GetComponent<CharacterWeapon>() == null)
            {
                rigidbody.isKinematic = true;
                rigidbody.interpolation = RigidbodyInterpolation.Extrapolate;
                rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
            }
        }

        GoTo(initialState);
    }

    public void GoTo(AnimID state)
    {
        GoTo(state, 0);
    }

    public void GoTo(AnimID state, float transitionTime)
    {
        string animID = state.ToString();

        Debug.Log("Animation: " + animID);

        _animator.CrossFade(animID, transitionTime);
    }

    private void SetAimIKTarget(CharacterComponent target)
    {
        Debug.Log("Aim IK Active");

        _aimIK.solver.target = target.GetRandomBodyPart();
        _aimIK.solver.IKPositionWeight = 1;
    }

    private void ResetAimIK()
    {
        _aimIK.solver.target = null;
        _aimIK.solver.IKPositionWeight = 0;
    }

    public void HandleEvent(CharacterEvent characterEvent, object eventData)
    {
        if (!_canReceive) { return; }

        bool valid = true;

        switch (characterEvent)
        {
            case CharacterEvent.TARGETING:
                SetAimIKTarget((CharacterComponent)eventData);
                break;
            case CharacterEvent.UNTARGETING:
                ResetAimIK();
                break;
            case CharacterEvent.DEATH:
            {
                SwitchToRagdoll(0.2f);
                GoTo(GetAnimation(characterEvent));
                _canReceive = false;
                valid = false;
                break;
            }
            default:
                break;
        }

        if(valid)
        {
            GoTo(GetAnimation(characterEvent));
        }
    }

    public bool CanReceiveCharacterEvents()
    {
        return _canReceive;
    }
}
