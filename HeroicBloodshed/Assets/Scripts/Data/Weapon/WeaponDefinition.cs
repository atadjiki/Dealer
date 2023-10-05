using System;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

[Serializable]
public struct DamageInfo
{
    public CharacterComponent caster;
    public CharacterComponent target;
    public int ActualDamage;
    public int BaseDamage;
    public bool IsCrit;
    public bool IsKill;
}

public struct WeaponDefinition
{
    public WeaponID ID;

    public PrefabID[] MuzzleVFX;

    public AudioID[] AttackSFX;

    public AudioID[] ReloadSFX;

    public int BaseDamage;

    public int CritDamage;

    public int Spread;

    public int Ammo;

    public float PlusOneThreshold; //0-1

    public DamageInfo CalculateDamage(CharacterComponent caster, CharacterComponent target)
    {
        CharacterDefinition casterDef = CharacterDefinition.Get(caster.GetID());

        DamageInfo damageInfo = new DamageInfo();
        damageInfo.caster = caster;
        damageInfo.target = target;

        //first, get a random amount of damage by applying the spread to the base damage
        int TotalDamage = UnityEngine.Random.Range(BaseDamage - Spread, BaseDamage + Spread + 1);

        //if our threshold is higher than the chance, award an extra point of damage 
        float PlusOneChance = UnityEngine.Random.Range(0, 1);
        if(PlusOneThreshold > PlusOneChance)
        {
            TotalDamage += 1;
        }

        //if shot is a crit, add the additional damage to the total 
        if (casterDef.RollCritChance())
        {
            Debug.Log("Critical Hit!");
            TotalDamage += CritDamage;
            damageInfo.IsCrit = true;
        }

        int targetHealth = target.GetHealth();

        if ((targetHealth - TotalDamage) <= 0)
        {
            Debug.Log("Kill!");
            damageInfo.IsKill = true;
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
                    MuzzleVFX = new PrefabID[]
                    {
                        PrefabID.VFX_MuzzleFlash_Auto_Medium,
                        PrefabID.VFX_MuzzleFlash_Smoke,
                        PrefabID.VFX_MuzzleFlash_Sparks,
                    },
                    AttackSFX = new AudioID[]
                    {
                        AudioID.SFX_SMG_1,
                    },
                    ReloadSFX = new AudioID[]
                    {
                        AudioID.SFX_Reload_Default,
                    },

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
                    MuzzleVFX = new PrefabID[]
                    {
                        PrefabID.VFX_MuzzleFlash_Default,
                        PrefabID.VFX_MuzzleFlash_Smoke,
                        PrefabID.VFX_MuzzleFlash_Sparks,
                    },
                    AttackSFX = new AudioID[]
                    {
                        AudioID.SFX_Pistol_1,
                        AudioID.SFX_Pistol_2,
                        AudioID.SFX_Pistol_3,
                        AudioID.SFX_Pistol_4,
                        AudioID.SFX_Pistol_5,
                        AudioID.SFX_Pistol_6,
                    },
                    ReloadSFX = new AudioID[]
                    {
                        AudioID.SFX_Reload_Default,
                    },
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
                    MuzzleVFX = new PrefabID[]
                    {
                        PrefabID.VFX_MuzzleFlash_Default,
                        PrefabID.VFX_MuzzleFlash_Smoke,
                        PrefabID.VFX_MuzzleFlash_Sparks,
                    },
                    AttackSFX = new AudioID[]
                    {
                        AudioID.SFX_Revolver_1,
                        AudioID.SFX_Revolver_2,
                        AudioID.SFX_Revolver_3,
                        AudioID.SFX_Revolver_4,
                        AudioID.SFX_Revolver_5,
                        AudioID.SFX_Revolver_6,
                    },
                    ReloadSFX = new AudioID[]
                    {
                        AudioID.SFX_Reload_Default,
                    },
                };
            default:
                {
                    return new WeaponDefinition();
                }
        }
    }
}
