using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Constants;

public static class PrefabHelper
{
    public static GameObject GetCharacterDecal()
    {
        return Resources.Load<GameObject>("Prefabs/Decals/Decal_Character");
    }

    public static GameObject GetCharacterCombatCanvas()
    {
        return Resources.Load<GameObject>("Prefabs/UI/CharacterCombatCanvas");
    }

    public static GameObject GetWeaponByID(CharacterConstants.WeaponID type)
    {
        switch(type)
        {
            case CharacterConstants.WeaponID.Revolver:
                return Resources.Load<GameObject>("Prefabs/Weapons/Model_Weapon_Revolver");
            case CharacterConstants.WeaponID.Pistol:
                return Resources.Load<GameObject>("Prefabs/Weapons/Model_Weapon_Pistol");
            default:
                return null;
        }
    }

    public static GameObject GetCharacterModel(CharacterConstants.ModelID ID)
    {
        switch(ID)
        {
            case CharacterConstants.ModelID.MAFIA_ITALIAN:
                return Resources.Load<GameObject>("Prefabs/Character/Model_Character_Mafia_Alternate");
            case CharacterConstants.ModelID.MAFIA_BRITISH:
                return Resources.Load<GameObject>("Prefabs/Character/Model_Character_Mafia_Default");
            case CharacterConstants.ModelID.DEA_FEMALE:
                return Resources.Load<GameObject>("Prefabs/Character/Model_Character_DEA_Alternate");
            case CharacterConstants.ModelID.DEA_MALE:
                return Resources.Load<GameObject>("Prefabs/Character/Model_Character_DEA_Default");
            default:
                return null;

        }
    }
}
