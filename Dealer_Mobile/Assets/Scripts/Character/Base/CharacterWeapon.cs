using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine;

public class CharacterWeapon : MonoBehaviour
{
    private CharacterConstants.WeaponID _ID;

    public void SetID(CharacterConstants.WeaponID ID)
    {
        _ID = ID;
    }

    public CharacterConstants.WeaponID GetID()
    {
        return _ID;
    }
    public virtual void OnAttack()
    {
        //play sound effects, etc here
    }
}
