using System.Collections;
using System.Collections.Generic;
using static Constants;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CharacterAnimator : MonoBehaviour, ICharacterEventReceiver
{
    private Animator _animator;

    private WeaponID _weaponID;

    private bool _canReceive = true;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void SwitchToRagdoll(DamageInfo damageInfo, float delay)
    {
        StartCoroutine(Coroutine_SwitchToRagdoll(damageInfo, delay));
    }

    private IEnumerator Coroutine_SwitchToRagdoll(DamageInfo damageInfo, float delay)
    {
        yield return new WaitForSeconds(delay);

        //Vector3 impactAngle;

        //if (damageInfo.caster != null)
        //{
        //    CharacterComponent caster = damageInfo.caster;
        //    Vector3 casterPos = caster.transform.position;
        //    Vector3 targetPos = this.transform.position;

        //    impactAngle = targetPos - casterPos;

        //    impactAngle = Vector3.Normalize(impactAngle + (this.transform.forward));
        //}
        //else
        //{
        //    impactAngle = this.transform.forward * -1;
        //}

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

        if (rigidbodies.Length > 0)
        {
            Rigidbody impactedBody = PickImpactedRigidBody();

            //float mass = impactedBody.mass;

            //Vector3 forceVector = impactAngle * mass * 10f;

            //impactedBody.AddForce(forceVector, ForceMode.Impulse);

            //Debug.Log("Bullet Force of : " + forceVector.magnitude + " on " + impactedBody.name);

            ProduceBloodSpray(impactedBody.transform);
        }
    }
    public Rigidbody PickImpactedRigidBody()
    {
        Rigidbody[] rigidbodies = GetComponentsInChildren<Rigidbody>();

        return rigidbodies[Random.Range(0, rigidbodies.Length)];
    }

    private void ProduceBloodSpray(Transform parentTransform)
    {
        int randomCount = Random.Range(1, 4);

        for (int i = 0; i < randomCount; i++)
        {
            StartCoroutine(Coroutine_ProduceBloodSpray(parentTransform));
        }
    }

    private IEnumerator Coroutine_ProduceBloodSpray(Transform parentTransform)
    {
        ResourceRequest resourceRequest = GetCharacterVFX(PrefabID.VFX_Bloodspray);

        yield return new WaitUntil(() => resourceRequest.isDone);

        GameObject prefab = (GameObject)resourceRequest.asset;

        GameObject particleObject = Instantiate<GameObject>(prefab, parentTransform);

        ParticleSystem particleSystem = prefab.GetComponent<ParticleSystem>();

        yield return new WaitForSecondsRealtime(particleSystem.main.duration);

        Destroy(particleObject);
    }

    public void Setup(AnimState initialState, WeaponID weapon)
    {
        _weaponID = weapon;

        int layerIndex = GetLayerByWeapon(_weaponID);

        _animator.SetLayerWeight(layerIndex, 1);

        Rigidbody[] rigidbodies = GetComponentsInChildren<Rigidbody>();

        foreach (Rigidbody rigidbody in rigidbodies)
        {
            rigidbody.isKinematic = true;
            rigidbody.interpolation = RigidbodyInterpolation.Extrapolate;
            rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        }

        GoTo(initialState);
    }

    public void GoTo(AnimState state)
    {
        GoTo(state, 0);
    }

    public void GoTo(AnimState state, float transitionTime)
    {
        string animID = state.ToString();

        Debug.Log("Animation: " + animID);
        _animator.CrossFade(animID, transitionTime);
    }

    public void HandleEvent(object eventData, CharacterEvent characterEvent)
    {
        if (!_canReceive) { return; }

        switch (characterEvent)
        {
            case CharacterEvent.HIT:
                HandleEvent_Hit(eventData);
                break;
            case CharacterEvent.KILLED:
                HandleEvent_Death(eventData);
                _canReceive = false;
                break;
            default:
                break;
        }
    }

    private void HandleEvent_Hit(object eventData)
    {
        Rigidbody impactedBody = PickImpactedRigidBody();

        ProduceBloodSpray(impactedBody.transform);
    }

    private void HandleEvent_Death(object eventData)
    {
        DamageInfo damageInfo = new DamageInfo();
        if (eventData != null)
        {
             damageInfo = (DamageInfo)eventData;
        }

        GoTo(AnimState.Death);
        SwitchToRagdoll(damageInfo, 0.1f);
    }

    public bool CanReceiveCharacterEvents()
    {
        return _canReceive;
    }
}
