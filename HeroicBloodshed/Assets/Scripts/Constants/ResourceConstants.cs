using System.Collections.Generic;
using UnityEngine;

public static partial class Constants
{
    public static ResourceID GetResourceID(WeaponID ID)
    {
        switch (ID)
        {
            case WeaponID.Revolver:
                return ResourceID.Model_Weapon_Revolver;
            case WeaponID.Pistol:
                return ResourceID.Model_Weapon_Pistol;
            case WeaponID.SMG:
                return ResourceID.Model_Weapon_SMG;
            default:
                return ResourceID.INVALID;
        }
    }

    public static ResourceID GetResourceID(ModelID ID)
    {
        switch(ID)
        {
            case ModelID.DEA_MALE:
                return ResourceID.Model_Character_DEA_Male;
            case ModelID.DEA_FEMALE:
                return ResourceID.Model_Character_DEA_Female;
            case ModelID.MAFIA_BRITISH:
                return ResourceID.Model_Character_Mafia_British;
            case ModelID.MAFIA_ITALIAN:
                return ResourceID.Model_Character_Mafia_Italian;
            default:
                return ResourceID.INVALID;
        }
    }

    public static ResourceID GetResourceID(InventoryItemID ID)
    {
        switch (ID)
        {
            default:
                return ResourceID.INVALID;
        }
    }

    public static string GetResourcePath(WeaponID ID) { return GetResourcePath(GetResourceID(ID)); }
    public static string GetResourcePath(ModelID ID) { return GetResourcePath(GetResourceID(ID)); }

    public static string GetResourcePath(ResourceID ID)
    {
        switch(ID)
        {
            //Character Model
            case ResourceID.Model_Character_DEA_Male:
                return "Prefabs/Character/Model_Character_DEA_Male";
            case ResourceID.Model_Character_DEA_Female:
                return "Prefabs/Character/Model_Character_DEA_Female";
            case ResourceID.Model_Character_Mafia_British:
                return "Prefabs/Character/Model_Character_Mafia_British";
            case ResourceID.Model_Character_Mafia_Italian:
                return "Prefabs/Character/Model_Character_Mafia_Italian";

            //Weapon Model
            case ResourceID.Model_Weapon_Revolver:
                return "Prefabs/Weapons/Model_Weapon_Revolver";
            case ResourceID.Model_Weapon_Pistol:
                return "Prefabs/Weapons/Model_Weapon_Pistol";
            case ResourceID.Model_Weapon_SMG:
                return "Prefabs/Weapons/Model_Weapon_SMG";

            //Decals
            case ResourceID.Decal_Character:
                return "Prefabs/Decals/Decal_Character";

            //Overhead
            case ResourceID.Encounter_CharacterUI_Canvas:
                return "Prefabs/Encounter/UI/Encounter_CurrentCharacter_Canvas";

            default:
                return null;
        }
    }

    //Every single prefab in the Resources folder must correspond to a unique ID
    public enum ResourceID
    {
        INVALID,
        //PREFABS

        //Character
        Model_Character_DEA_Male,
        Model_Character_DEA_Female,
        Model_Character_Mafia_British,
        Model_Character_Mafia_Italian,

        //Decals
        Decal_Character,

        //Encounter Prefabs

        //---Camera
        CM_Encounter_Main,
        CM_Encounter_Character_Main,

        //---Character
        Anchor_CharacterOverhead,
        AudioSource_Character_Male,
        AudioSource_Character_Female,

        //---Environment
        Environment_Encounter_LoadingDock,

        //---Managers,
        Manager_Encounter_Audio,
        Manager_Encounter_CameraRig,
        Manager_Encounter_UI,

        //---UI
        Encounter_AbilitySelect_Canvas,
        Encounter_AbilitySelect_Item,
        Encounter_CharacterUI_Canvas,
        Encounter_CharacterUI_Item,
        Encounter_CurrentCharacter_Canvas,
        Encounter_CurrentCharacter_Item,
        Encounter_EventBanner_Canvas,
        Encounter_LoseDialog_Canvas,
        Encounter_ScreenFade_Canvas,
        Encounter_StateDetail_Canvas,
        Encounter_TargetSelect_Canvas,
        Encounter_TargetSelect_Item,
        Encounter_TeamBanner_Canvas,
        Encounter_WinDialog_Canvas,
        Encounter_WinDialog_Item,

        //Lighting
        CharacterLightRig,

        //Markers
        Marker_Default,
        Marker_Player,
        Marker_Enemy,

        //Player
        Inventory,

        //Safehouse
        City,


        //Weapons
        Model_Weapon_Pistol,
        Model_Weapon_Revolver,
        Model_Weapon_SMG,

        //SPRITE

        Cursor_Busy,
        Cursor_Cancel,
        Cursor_Default,
        Cursor_Help,
        Cursor_Interact,

        Icon_White_Balaclava,
        Icon_White_Bullet,
        Icon_White_Bullets,
        Icon_White_Cancel,
        Icon_White_Cog,
        Icon_White_Crosshair,
        Icon_White_Cycle,
        Icon_White_Halt,
        Icon_White_Mugshot,
        Icon_White_Pistol,
        Icon_White_PoliceBadge,
        Icon_White_Reload,
        Icon_White_Revolver,
        Icon_White_Select,
        Icon_White_ShotgunShells,
        Icon_White_Skip,
        Icon_White_SMG,
        Icon_White_Square,
        Icon_White_Usable,
    }
}
