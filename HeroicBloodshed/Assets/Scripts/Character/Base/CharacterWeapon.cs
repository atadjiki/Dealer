using System.Collections;
using System.Collections.Generic;
using static Constants;
using UnityEngine;

public class CharacterWeapon : MonoBehaviour
{
    private WeaponID _ID;

    public void SetID(WeaponID ID)
    {
        _ID = ID;
    }

    public WeaponID GetID()
    {
        return _ID;
    }
    public virtual void OnAttack()
    {
        //play sound effects, etc here
    }
}
