using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

public struct WeaponAttackDefinition
{
    public WeaponID ID;

    public int MinShots;

    public int MaxShots;

    public float TimeBetweenShots;

    public int CalculateShotCount()
    {
        return Random.Range(MinShots, MaxShots);
    }

    public static WeaponAttackDefinition Get(WeaponID _ID)
    {
        switch (_ID)
        {
            case WeaponID.Pistol:
            return new WeaponAttackDefinition()
            {
                ID = _ID,
                MinShots = 3,
                MaxShots = 5,
                TimeBetweenShots = 0.33f
            };
            case WeaponID.Revolver:
            return new WeaponAttackDefinition()
            {
                ID = _ID,
                MinShots = 2,
                MaxShots = 3,
                TimeBetweenShots = 0.75f
            };
            case WeaponID.SMG:
            return new WeaponAttackDefinition()
            {
                ID = _ID,
                MinShots = 7,
                MaxShots = 10,
                TimeBetweenShots = 0.1f
            };
            default:
                return new WeaponAttackDefinition();
        }
    }
}
