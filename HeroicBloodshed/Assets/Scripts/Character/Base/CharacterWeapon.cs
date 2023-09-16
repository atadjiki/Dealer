using System.Collections;
using System.Collections.Generic;
using static Constants;
using UnityEngine;

public class CharacterWeapon : MonoBehaviour
{
    private WeaponID _ID;
    protected int _ammo = 0;

    public void Setup(WeaponID ID)
    {
        _ID = ID;
        Reload();
    }

    public WeaponID GetID()
    {
        return _ID;
    }
    public virtual void OnAttack()
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

    public void Reload()
    {
        _ammo = GetMaxAmmo();
    }
}
