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

    public void SwitchToRagdoll(DamageInfo damageInfo)
    {
        CharacterComponent characterComponent = damageInfo.caster;

        Vector3 casterPos = characterComponent.transform.position;
        Vector3 targetPos = this.transform.position;

        Vector3 impactAngle = targetPos - casterPos;

        impactAngle = Vector3.Normalize(impactAngle + (this.transform.forward));

        _animator.enabled = false;

        foreach (Collider collider in GetComponentsInChildren<Collider>())
        {
            if (!collider.isTrigger)
            {
                collider.enabled = true;
            }
        }

        foreach (Rigidbody rigidbody in GetComponentsInChildren<Rigidbody>())
        {
            float mass = rigidbody.mass;

            Vector3 forceVector = impactAngle * mass;

            Debug.Log("Force Vector: " + forceVector.ToString());

            rigidbody.isKinematic = false;
            rigidbody.AddForce(forceVector, ForceMode.Force);

        }
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

    public void HandleEvent(object eventData, CharacterEvent characterEvent)
    {
        switch (characterEvent)
        {
            case CharacterEvent.DEAD:
                HandleEvent_Death(eventData);
                break;
            default:
                break;
        }
    }

    private void HandleEvent_Death(object eventData)
    {
        DamageInfo damageInfo = (DamageInfo)eventData;

        GoTo(AnimState.Dead);
        SwitchToRagdoll(damageInfo);
    }
}
