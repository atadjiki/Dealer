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

    public static GameObject GetWeaponByID(CharacterConstants.Weapon type)
    {
        switch(type)
        {
            case CharacterConstants.Weapon.Revolver:
                return Resources.Load<GameObject>("Prefabs/Weapons/Model_Weapon_Revolver");
            case CharacterConstants.Weapon.Pistol:
                return Resources.Load<GameObject>("Prefabs/Weapons/Model_Weapon_Pistol");
            default:
                return null;
        }
    }

    public static GameObject GetCharacterModelByTeam(CharacterConstants.Team team, CharacterConstants.CharacterType type)
    {
        switch(team)
        {
            case CharacterConstants.Team.Mafia:
                return GetMafiaCharacterPrefab(type);
            case CharacterConstants.Team.DEA:
                return GetDEACharacterPrefab(type);
            default:
                return null;

        }
    }

    public static GameObject GetMafiaCharacterPrefab(CharacterConstants.CharacterType type)
    {
        switch (type)
        {
            case CharacterConstants.CharacterType.Alternate:
                return Resources.Load<GameObject>("Prefabs/Character/Model_Character_Mafia_Alternate");
            default:
                return Resources.Load<GameObject>("Prefabs/Character/Model_Character_Mafia_Default");
        }
    }

    public static GameObject GetDEACharacterPrefab(CharacterConstants.CharacterType type)
    {
        switch (type)
        {
            case CharacterConstants.CharacterType.Alternate:
                return Resources.Load<GameObject>("Prefabs/Character/Model_Character_DEA_Alternate");
            default:
                return Resources.Load<GameObject>("Prefabs/Character/Model_Character_DEA_Default");
        }
    }
}
