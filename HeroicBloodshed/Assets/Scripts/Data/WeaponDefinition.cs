using System;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

public struct DamageInfo
{
    public int ActualDamage;
    public int BaseDamage;
}

public struct WeaponDefinition
{
    public WeaponID ID;

    public int BaseDamage;

    public int CritDamage;

    public int Spread;

    public int Ammo;

    public float PlusOneThreshold; //0-1

    public DamageInfo CalculateDamage(bool IsCrit = false)
    {
        DamageInfo damageInfo = new DamageInfo();

        //first, get a random amount of damage by applying the spread to the base damage
        int TotalDamage = UnityEngine.Random.Range(BaseDamage - Spread, BaseDamage + Spread + 1);

        //if our threshold is higher than the chance, award an extra point of damage 
        float PlusOneChance = UnityEngine.Random.Range(0, 1);
        if(PlusOneThreshold > PlusOneChance)
        {
            TotalDamage += 1;
        }

        //if shot is a crit, add the additional damage to the total 
        if(IsCrit)
        {
            Debug.Log("Critical Hit!");
            TotalDamage += CritDamage;
        }

        Debug.Log("Damage: " + TotalDamage);
        damageInfo.ActualDamage = TotalDamage;
        damageInfo.BaseDamage = BaseDamage;

        return damageInfo;
    }

    //yep these are hardcoded :)
    public static WeaponDefinition Get(WeaponID uniqueID)
    {
        switch (uniqueID)
        {
            case WeaponID.SMG:
                return new WeaponDefinition()
                {
                    ID = WeaponID.SMG,
                    BaseDamage = 3,
                    CritDamage = 0,
                    Spread = 2,
                    Ammo = 2,
                    PlusOneThreshold = 0.25f,
                };
            case WeaponID.Pistol:
                return new WeaponDefinition()
                {
                    ID = WeaponID.Pistol,
                    BaseDamage = 3,
                    CritDamage = 2,
                    Spread = 1,
                    Ammo = 3,
                    PlusOneThreshold = 0,
                };
            case WeaponID.Revolver:
                return new WeaponDefinition()
                {
                    ID = WeaponID.Revolver,
                    BaseDamage = 3,
                    CritDamage = 2,
                    Spread = 1,
                    Ammo = 3,
                    PlusOneThreshold = 0,
                };
            default:
                {
                    return new WeaponDefinition()
                    {
                        ID = WeaponID.None,
                        BaseDamage = 0,
                        CritDamage = 0,
                        Spread = 0,
                        Ammo = 0,
                        PlusOneThreshold = 0,
                    };
                }
        }
    }
}
