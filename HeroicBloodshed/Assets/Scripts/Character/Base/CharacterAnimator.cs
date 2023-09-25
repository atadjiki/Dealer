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
        Vector3 impactAngle;

        if (damageInfo.caster != null)
        {
            CharacterComponent caster = damageInfo.caster;
            Vector3 casterPos = caster.transform.position;
            Vector3 targetPos = this.transform.position;

            impactAngle = targetPos - casterPos;

            impactAngle = Vector3.Normalize(impactAngle + (this.transform.forward));
        }
        else
        {
            impactAngle = this.transform.forward * -1;
        }

        _animator.enabled = false;

        foreach (Collider collider in GetComponentsInChildren<Collider>())
        {
            if (!collider.isTrigger)
            {
                collider.enabled = true;
            }
        }

        Rigidbody[] rigidbodies = GetComponentsInChildren<Rigidbody>();

        foreach (Rigidbody rigidbody in rigidbodies)
        {
            rigidbody.isKinematic = false;
        }

        if(rigidbodies.Length > 0)
        {
            Rigidbody impactedBody = rigidbodies[Random.Range(0, rigidbodies.Length)];

            float mass = impactedBody.mass;

            Vector3 forceVector = impactAngle * mass * 10f;

            impactedBody.AddForce(forceVector, ForceMode.Impulse);

            Debug.Log("Bullet Force of : " + forceVector.magnitude + " on " + impactedBody.name);

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
            case CharacterEvent.KILLED:
                HandleEvent_Death(eventData);
                break;
            default:
                break;
        }
    }

    private void HandleEvent_Death(object eventData)
    {
        DamageInfo damageInfo = new DamageInfo();
        if (eventData != null)
        {
             damageInfo = (DamageInfo)eventData;
        }

        GoTo(AnimState.Dead);
        SwitchToRagdoll(damageInfo);
    }
}
