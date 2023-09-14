using System;
using System.Collections.Generic;
using static Constants;

public struct WeaponDefinition
{
    public WeaponID ID;

    public Tuple<int,int> BaseDamage;

    public int Ammo;

    public int GetRandomDamage()
    {
        return UnityEngine.Random.Range(BaseDamage.Item1, BaseDamage.Item2);
    }

    //yep these are hardcoded :)
    public static WeaponDefinition Get(WeaponID uniqueID)
    {
        switch (uniqueID)
        {
            case WeaponID.Pistol:
                return new WeaponDefinition()
                {
                    ID = WeaponID.Pistol,
                    BaseDamage = new Tuple<int, int>(3, 5),
                    Ammo = 3
                };
            case WeaponID.Revolver:
                return new WeaponDefinition()
                {
                    ID = WeaponID.Revolver,
                    BaseDamage = new Tuple<int, int>(3, 5),
                    Ammo = 3
                };
            default:
                {
                    return new WeaponDefinition()
                    {
                        ID = WeaponID.None,
                        BaseDamage = new Tuple<int, int>(0,0),
                        Ammo = 0
                    };
                }
        }
    }
}
