using System.Collections.Generic;
using UnityEngine;

public static partial class Constants
{

    public static PrefabID GetPrefabID(ModelID ID)
    {
        switch(ID)
        {
            case ModelID.MALE_DEA:
                return PrefabID.Model_Character_Male_DEA;
            case ModelID.MALE_GENERIC:
                return PrefabID.Model_Character_Male_Generic;
            case ModelID.MALE_GENERIC_ENEMY:
                return PrefabID.Model_Character_Male_Generic_Enemy;
            default:
                return PrefabID.INVALID;
        }
    }

    public static PrefabID GetPrefabID(WeaponID ID)
    {
        switch(ID)
        {
            case WeaponID.Pistol:
                return PrefabID.Model_Weapon_Pistol;
            case WeaponID.Revolver:
                return PrefabID.Model_Weapon_Revolver;
            case WeaponID.SMG:
                return PrefabID.Model_Weapon_SMG;
            default:
                return PrefabID.INVALID;
        }
    }

    public static ResourceRequest GetCameraRig()
    {
        return GetPrefab(PrefabCategory.Camera, PrefabSubcategory.Camera, PrefabID.CameraRig);
    }

    public static ResourceRequest GetEncounterUI(PrefabID ID)
    {
        return GetPrefab(PrefabCategory.Encounter, PrefabSubcategory.UI, ID);
    }

    public static ResourceRequest GetCharacterComponent(PrefabID ID)
    {
        return GetPrefab(PrefabCategory.Character, PrefabSubcategory.Components, ID);
    }

    public static ResourceRequest GetCharacterVFX(PrefabID ID)
    {
        return GetPrefab(PrefabCategory.Character, PrefabSubcategory.VFX, ID);
    }

    public static ResourceRequest GetCharacterModel(ModelID ID)
    {
        return GetPrefab(PrefabCategory.Character, PrefabSubcategory.Model, GetPrefabID(ID));
    }

    public static ResourceRequest GetWeaponFX(PrefabID ID)
    {
        return GetPrefab(PrefabCategory.Weapon, PrefabSubcategory.VFX, ID);
    }

    public static ResourceRequest GetWeaponModel(WeaponID ID)
    {
        return GetPrefab(PrefabCategory.Weapon, PrefabSubcategory.Model, GetPrefabID(ID));
    }

    public static ResourceRequest GetAudioSource(CharacterID characterID)
    {
        if(GetGender(characterID) == GenderID.Male)
        {
            return GetPrefab(PrefabCategory.Character, PrefabSubcategory.Components, PrefabID.Character_AudioSource_Male);
        }
        else
        {
            return GetPrefab(PrefabCategory.Character, PrefabSubcategory.Components, PrefabID.Character_AudioSource_Female);
        }
    }

    public static ResourceRequest GetEnvironmentVFX(PrefabID ID)
    {
        return GetPrefab(PrefabCategory.Environment, PrefabSubcategory.VFX, ID);
    }

    private static ResourceRequest GetPrefab(PrefabCategory category, PrefabSubcategory subcategory, PrefabID ID)
    {
        string fileName =
            "Prefabs/" +
            category + "/" +
            subcategory + "/" +
            ID.ToString();

       // Debug.Log("Loading Prefab: " + fileName);

        ResourceRequest resourceRequest = Resources.LoadAsync<GameObject>(fileName);

        return resourceRequest;
    }

    public static ResourceRequest GetTexture(TextureID ID)
    {
        string fileName = "Textures/" + ID.ToString();

        //Debug.Log("Loading Texture: " + fileName);

        ResourceRequest resourceRequest = Resources.LoadAsync<Texture2D>(fileName);

        return resourceRequest;
    }

    public static ResourceRequest GetTexture(AbilityID ID)
    {
        switch(ID)
        {
            case AbilityID.Attack:
                return GetTexture(TextureID.Icon_Crosshair);
            case AbilityID.Reload:
                return GetTexture(TextureID.Icon_Cycle);
            case AbilityID.SkipTurn:
                return GetTexture(TextureID.Icon_Cancel);
            case AbilityID.Heal:
                return GetTexture(TextureID.Icon_Medicine);
            default:
                return GetTexture(TextureID.Icon_Square);
        }
    }

    public static ResourceRequest GetAudioClip(AudioID ID)
    {
        string fileName = "Audio/" + ID.ToString();

        //Debug.Log("Loading Audio Clip: " + fileName);

        ResourceRequest resourceRequest = Resources.LoadAsync<AudioClip>(fileName);

        return resourceRequest;
    }
}
