using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine;

public class CharacterWeapon : MonoBehaviour
{
    private Game.WeaponID _ID;

    public void SetID(Game.WeaponID ID)
    {
        _ID = ID;
    }

    public Game.WeaponID GetID()
    {
        return _ID;
    }
    public virtual void OnAttack()
    {
        //play sound effects, etc here
    }
}
