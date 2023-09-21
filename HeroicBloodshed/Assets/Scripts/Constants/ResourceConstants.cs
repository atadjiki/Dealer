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

            //Encounter Camera
            case ResourceID.CM_Encounter_Main:
                return "Prefabs/Encounter/Camera/CM_Encounter_Main";
            case ResourceID.CM_Encounter_Character_Main:
                return "Prefabs/Encounter/Camera/CM_Encounter_Character_Main";

            //Encounter Character
            case ResourceID.Anchor_CharacterOverhead:
                return "Prefabs/Encounter/Character/Anchor_CharacterOverhead";
            case ResourceID.AudioSource_Character_Male:
                return "Prefabs/Encounter/Character/AudioSource_Character_Male";
            case ResourceID.AudioSource_Character_Female:
                return "Prefabs/Encounter/Character/AudioSource_Character_Female";

            //Encounter Managers
            case ResourceID.Manager_Encounter_Audio:
                return "Prefabs/Encounter/Managers/Manager_Encounter_Audio";
            case ResourceID.Manager_Encounter_CameraRig:
                return "Prefabs/Encounter/Managers/Manager_Encounter_CameraRig";
            case ResourceID.Manager_Encounter_UI:
                return "Prefabs/Encounter/Managers/Manager_Encounter_UI";

            //Encounter UI
            case ResourceID.Encounter_AbilitySelect_Canvas:
                return "Prefabs/Encounter/UI/Encounter_AbilitySelect_Canvas";
            case ResourceID.Encounter_AbilitySelect_Item:
                return "Prefabs/Encounter/UI/Encounter_AbilitySelect_Item";
            case ResourceID.Encounter_CharacterUI_Canvas:
                return "Prefabs/Encounter/UI/Encounter_CurrentCharacter_Canvas";
            case ResourceID.Encounter_CharacterUI_Item:
                return "Prefabs/Encounter/UI/Encounter_CharacterUI_Item";
            case ResourceID.Encounter_CurrentCharacter_Canvas:
                return "Prefabs/Encounter/UI/Encounter_CurrentCharacter_Canvas";
            case ResourceID.Encounter_CurrentCharacter_Item:
                return "Prefabs/Encounter/UI/Encounter_CurrentCharacter_Item";
            case ResourceID.Encounter_EventBanner_Canvas:
                return "Prefabs/Encounter/UI/Encounter_EventBanner_Canvas";
            case ResourceID.Encounter_LoseDialog_Canvas:
                return "Prefabs/Encounter/UI/Encounter_LoseDialog_Canvas";
            case ResourceID.Encounter_ScreenFade_Canvas:
                return "Prefabs/Encounter/UI/Encounter_ScreenFade_Canvas";
            case ResourceID.Encounter_StateDetail_Canvas:
                return "Prefabs/Encounter/UI/Encounter_StateDetail_Canvas";
            case ResourceID.Encounter_TargetSelect_Canvas:
                return "Prefabs/Encounter/UI/Encounter_TargetSelect_Canvas";
            case ResourceID.Encounter_TargetSelect_Item:
                return "Prefabs/Encounter/UI/Encounter_TargetSelect_Item";
            case ResourceID.Encounter_TeamBanner_Canvas:
                return "Prefabs/Encounter/UI/Encounter_TeamBanner_Canvas";
            case ResourceID.Encounter_WinDialog_Canvas:
                return "Prefabs/Encounter/UI/Encounter_WinDialog_Canvas";
            case ResourceID.Encounter_WinDialog_Item:
                return "Prefabs/Encounter/UI/Encounter_WinDialog_Item";

            //Icons
            case ResourceID.Icon_White_Balaclava:
                return "Sprites/Icons/White/Icon_White_Balaclava";
            case ResourceID.Icon_White_Bullet:
                return "Sprites/Icons/White/Icon_White_Bullet";
            case ResourceID.Icon_White_Bullets:
                return "Sprites/Icons/White/Icon_White_Bullets";
            case ResourceID.Icon_White_Cancel:
                return "Sprites/Icons/White/Icon_White_Cancel";
            case ResourceID.Icon_White_Cog:
                return "Sprites/Icons/White/Icon_White_Cog";
            case ResourceID.Icon_White_Crosshair:
                return "Sprites/Icons/White/Icon_White_Crosshair";
            case ResourceID.Icon_White_Cycle:
                return "Sprites/Icons/White/Icon_White_Cycle";
            case ResourceID.Icon_White_Halt:
                return "Sprites/Icons/White/Icon_White_Halt";
            case ResourceID.Icon_White_Mugshot:
                return "Sprites/Icons/White/Icon_White_Mugshot";
            case ResourceID.Icon_White_Pistol:
                return "Sprites/Icons/White/Icon_White_Pistol";
            case ResourceID.Icon_White_PoliceBadge:
                return "Sprites/Icons/White/Icon_White_PoliceBadge";
            case ResourceID.Icon_White_Reload:
                return "Sprites/Icons/White/Icon_White_Reload";
            case ResourceID.Icon_White_Revolver:
                return "Sprites/Icons/White/Icon_White_Revolver";
            case ResourceID.Icon_White_Select:
                return "Sprites/Icons/White/Icon_White_Select";
            case ResourceID.Icon_White_ShotgunShells:
                return "Sprites/Icons/White/Icon_White_ShotgunShells";
            case ResourceID.Icon_White_Skip:
                return "Sprites/Icons/White/Icon_White_Skip";
            case ResourceID.Icon_White_SMG:
                return "Sprites/Icons/White/Icon_White_SMG";
            case ResourceID.Icon_White_Square:
                return "Sprites/Icons/White/Icon_White_Square";
            case ResourceID.Icon_White_Usable:
                return "Sprites/Icons/White/Icon_White_Usable";


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
