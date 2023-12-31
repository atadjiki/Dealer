using System.Collections;
using System.Collections.Generic;
using static Constants;
using UnityEngine;

public class CharacterWeapon : MonoBehaviour, ICharacterEventReceiver
{
    protected WeaponID _ID;
    protected CharacterID _characterID;
    protected int _ammo = 0;

    private bool _canReceive = true;

    public virtual void Setup(CharacterID characterID, WeaponID weaponID)
    {
        _ID = weaponID;
        _characterID = characterID;
        _ammo = GetMaxAmmo();
    }

    public void HandleEvent(CharacterEvent characterEvent, object eventData)
    {
        if (!_canReceive) { return; }

        switch (characterEvent)
        {
            case CharacterEvent.FIRE:
                HandleEvent_Fire();
                break;
            case CharacterEvent.DEATH:
                HandleEvent_Death();
                _canReceive = false;
                break;
            default:
                break;
        }
    }

    protected virtual void HandleEvent_Fire()
    {

    }

    private void HandleEvent_Death()
    {
        Rigidbody rigidbody = GetComponent<Rigidbody>();
        rigidbody.isKinematic = false;

        this.transform.parent = null;

        StartCoroutine(Coroutine_DestroyAfterTime(5.0f));
    }

    private IEnumerator Coroutine_DestroyAfterTime(float duration)
    {
        yield return new WaitForSecondsRealtime(duration);

        Destroy(this.gameObject);
    }

    public WeaponID GetID()
    {
        return _ID;
    }
    public virtual void OnAbility(AbilityID ability)
    {
    }

    public int GetRemainingAmmo()
    {
        return _ammo;
    }

    public int GetMaxAmmo()
    {
        WeaponDefinition weaponDefinition = WeaponDefinition.Get(GetID());

        return weaponDefinition.Ammo;
    }

    public bool HasAmmo()
    {
        return _ammo > 0;
    }

    public bool CanReceiveCharacterEvents()
    {
        return _canReceive;
    }
}
