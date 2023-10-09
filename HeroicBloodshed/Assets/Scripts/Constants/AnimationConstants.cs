using UnityEngine;

public static partial class Constants
{

    public static bool IsUnarmedAnim(AnimState state)
    {
        switch(state)
        {
            case AnimState.Dead:
            case AnimState.Interact:
            case AnimState.Heal:
            case AnimState.SkipTurn:
                return true;
            default:
                return false;
        }
    }

    public static AnimID GetUnarmedAnim(AnimState state)
    {
        switch(state)
        {
            case AnimState.Dead:
                return AnimID.Anim_Character_Death;
            case AnimState.Interact:
                return AnimID.Anim_Character_ButtonPush;
            case AnimState.Heal:
                return AnimID.Anim_Character_ShoulderRub;
            case AnimState.SkipTurn:
                return AnimID.Anim_Character_WipingSweat;
            default:
                return AnimID.Anim_Character_Idle;
        }
    }

    public static AnimID GetAnimByWeaponType(WeaponID Weapon, AnimState state)
    {
        switch (Weapon)
        {
            case WeaponID.Pistol:
            case WeaponID.Revolver:
            case WeaponID.SMG:
                return GetPistolAnim(state);

            default:
                return AnimID.Anim_Character_Idle;
        }
    }

    public static AnimID GetPistolAnim(AnimState state)
    {
        switch (state)
        {
            case AnimState.Idle:
                return AnimID.Anim_Character_Pistol_Idle;
            case AnimState.Walking:
                return AnimID.Anim_Character_Pistol_Walking;
            case AnimState.Running:
                return AnimID.Anim_Character_Pistol_Running;
            case AnimState.Attack_Single:
                return AnimID.Anim_Character_Pistol_Attack_Single;
            case AnimState.Hit_Light:
                return AnimID.Anim_Character_Pistol_Hit_Light;
            case AnimState.Hit_Medium:
                return AnimID.Anim_Character_Pistol_Hit_Medium;
            case AnimState.Hit_Heavy:
                return AnimID.Anim_Character_Pistol_Hit_Heavy;
            case AnimState.Reload:
                return AnimID.Anim_Character_Pistol_Reload;
            default:
                return AnimID.Anim_Character_Pistol_Idle;
        }
    }

}