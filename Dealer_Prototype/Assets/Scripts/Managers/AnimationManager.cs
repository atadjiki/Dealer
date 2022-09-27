using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using Constants;
using UnityEngine;

public class AnimationManager : Singleton<AnimationManager>
{
    [Header("Controllers")]
    [SerializeField] public AnimatorController DefaultController;
    [SerializeField] public AnimatorController CombatController_Pistol;

    public AnimatorController GetControllerByWeaponID(Enumerations.WeaponID weaponID)
    {
        switch(weaponID)
        {
            case Enumerations.WeaponID.Glock:
            case Enumerations.WeaponID.Deagle:
            case Enumerations.WeaponID.Revolver:
            case Enumerations.WeaponID.Uzi:
                return CombatController_Pistol;
            default:
                return DefaultController;
        }
    }
}
