using System.Collections;
using System.Collections.Generic;
using static Constants;
using UnityEngine;

public class CharacterWeapon : MonoBehaviour, ICharacterEventReceiver
{
    protected WeaponID _ID;
    protected int _ammo = 0;

    public virtual void Setup(WeaponID ID)
    {
        _ID = ID;
        _ammo = GetMaxAmmo();
    }

    public void HandleEvent(Constants.CharacterEvent characterEvent)
    {
        switch (characterEvent)
        {
            case CharacterEvent.DEAD:
                this.transform.parent = null;
                break;
            default:
                break;
        }
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
}
