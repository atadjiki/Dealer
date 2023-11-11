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

    public enum PrefabCategory
    {
        Camera,
        Character,
        Encounter,
        Environment,
        Weapon
    }

    public enum PrefabSubcategory
    {
        Components,
        Model,
        VFX,
        Camera,
        Manager,
        UI
    }

    //Every single prefab in the Resources folder must correspond to a unique ID
    public enum PrefabID
    {
        INVALID,

        //Camera
        CameraRig,

        //Character
        Character_Anchor_Overhead,
        Character_AudioSource_Female,
        Character_AudioSource_Male,
        Character_Decal,
        Character_Marker_Default,
        Character_Marker_Enemy,
        Character_Marker_Player,
        Character_Navigator,
        Character_Outliner,

        //Environment
        Environment_DrugHideout,
        
        Encounter_Manager_Audio,
        //Encounter_Manager_CameraRig,
        Encounter_Manager_UI,
        Encounter_UI_AbilitySelect_Canvas,
        Encounter_UI_AbilitySelect_Item,
        Encounter_UI_CharacterUI_Canvas,
        Encounter_UI_CharacterUI_Item,
        Encounter_UI_CurrentCharacter_Canvas,
        Encounter_UI_CurrentCharacter_Item,
        Encounter_UI_EventBanner_Canvas,
        Encounter_UI_LoseDialog_Canvas,
        Encounter_UI_ScreenFade_Canvas,
        Encounter_UI_StateDetail_Canvas,
        Encounter_UI_TargetSelect_Canvas,
        Encounter_UI_TargetSelect_Item,
        Encounter_UI_TeamBanner_Canvas,
        Encounter_UI_WinDialog_Canvas,
        Encounter_UI_WinDialog_Item,

        //Model

        //Characters
        Model_Character_Male_DEA,
        Model_Character_Male_Generic,
        Model_Character_Male_Generic_Enemy,

        //Weapons
        Model_Weapon_Pistol,
        Model_Weapon_Revolver,
        Model_Weapon_SMG,

        //VFX
        VFX_Cartridge_Auto,
        VFX_Cartridge_Pistol,
        VFX_Cartridge_Shotgun,

        VFX_Explosion_Default,

        VFX_Impact_Concrete,
        VFX_Impact_Ground,
        VFX_Impact_Metal,
        VFX_Impact_Wood,

        VFX_MuzzleFlash_Auto_Large,
        VFX_MuzzleFlash_Auto_Medium,
        VFX_MuzzleFlash_Auto_Small,
        VFX_MuzzleFlash_Default,
        VFX_MuzzleFlash_Smoke,
        VFX_MuzzleFlash_Sparks,

        VFX_Smoke_Dense,
        VFX_Smoke_Light,

        VFX_Bloodspray,

        //LineRenderers
        LineRenderer_Path,
        LineRenderer_Radius,
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

    public enum TextureID
    {
        //Cursors
        Cursor_Busy,
        Cursor_Cancel,
        Cursor_Default,
        Cursor_Help,
        Cursor_Interact,

        //Icons
        Icon_Balaclava,
        Icon_Bullet,
        Icon_Bullets,
        Icon_Cancel,
        Icon_Cog,
        Icon_Crosshair,
        Icon_Cycle,
        Icon_Halt,
        Icon_Medicine,
        Icon_Mugshot,
        Icon_Pistol,
        Icon_PoliceBadge,
        Icon_Reload,
        Icon_Revolver,
        Icon_Select,
        Icon_ShotgunShells,
        Icon_Skip,
        Icon_Square,
        Icon_Usable,

        //Image
        Image_City,
        Image_Technical_Difficulties,
    }

    public static ResourceRequest GetAudioClip(AudioID ID)
    {
        string fileName = "Audio/" + ID.ToString();

        //Debug.Log("Loading Audio Clip: " + fileName);

        ResourceRequest resourceRequest = Resources.LoadAsync<AudioClip>(fileName);

        return resourceRequest;
    }

    public enum AudioID
    {
        SFX_Pistol_1,
        SFX_Pistol_2,
        SFX_Pistol_3,
        SFX_Pistol_4,
        SFX_Pistol_5,
        SFX_Pistol_6,

        SFX_Revolver_1,
        SFX_Revolver_2,
        SFX_Revolver_3,
        SFX_Revolver_4,
        SFX_Revolver_5,
        SFX_Revolver_6,

        SFX_SMG_1,

        SFX_Reload_Default,
    }
}
