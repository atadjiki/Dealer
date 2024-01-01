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

    protected WeaponAttackDefinition _attackDef;


    public virtual void Setup(CharacterID characterID, WeaponID weaponID)
    {
        _ID = weaponID;
        _characterID = characterID;
        _ammo = GetMaxAmmo();

        _attackDef = WeaponAttackDefinition.Get(_ID);
    }

    public void HandleEvent(CharacterEvent characterEvent, object eventData = null)
    {
        if (!_canReceive) { return; }

        switch (characterEvent)
        {
            case CharacterEvent.FIRE:
            if (HasAmmo())
            {
                HandleEvent_Fire();
            }
            break;
            case CharacterEvent.RELOAD:
            if (CanReload())
            {
                HandleEvent_Reload();
            }
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

    protected virtual void HandleEvent_Reload()
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

    public bool CanReload()
    {
        return GetRemainingAmmo() != GetMaxAmmo();
    }

    public bool CanReceiveCharacterEvents()
    {
        return _canReceive;
    }

    public WeaponAttackDefinition GetWeaponAttackDefinition()
    {
        return _attackDef;
    }
}
