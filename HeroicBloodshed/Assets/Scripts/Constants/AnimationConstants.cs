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

    public static AnimID GetAnimation(CharacterEvent characterEvent)
    {
        switch(characterEvent)
        {
            case CharacterEvent.IDLE:
                return AnimID.Idle;
            case CharacterEvent.FIRE:
                return AnimID.Fire;
            case CharacterEvent.MELEE:
                return AnimID.Melee;
            case CharacterEvent.RELOAD:
                return AnimID.Reload;
            case CharacterEvent.GRENADE:
                return AnimID.Grenade;
            case CharacterEvent.SKIP_TURN:
                return AnimID.Skip_Turn;
            case CharacterEvent.HEAL:
                return AnimID.Heal;
            case CharacterEvent.HIT_LIGHT:
                return AnimID.Hit_Light;
            case CharacterEvent.HIT_HARD:
                return AnimID.Hit_Hard;
            case CharacterEvent.MOVING:
                return AnimID.Moving;
            case CharacterEvent.DEATH:
                return AnimID.Death;

            default:
                return AnimID.Idle;
        }
    }
}