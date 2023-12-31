using UnityEngine;

public static partial class Constants
{
    public static int GetLayerByWeapon(WeaponID weaponID)
    {
        switch(weaponID)
        {
            case WeaponID.Pistol:
                return 1;
            case WeaponID.Revolver:
                return 2;
            case WeaponID.SMG:
                return 3;
            default:
                return 0;
        }
    }
}