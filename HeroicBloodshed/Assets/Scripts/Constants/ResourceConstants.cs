using System.Collections.Generic;
using UnityEngine;

public static partial class Constants
{

    public static PrefabID GetPrefabID(ModelID ID)
    {
        switch(ID)
        {
            case ModelID.DEA_MALE:
                return PrefabID.Model_Character_DEA_Male;
            case ModelID.DEA_FEMALE:
                return PrefabID.Model_Character_DEA_Female;
            case ModelID.MAFIA_BRITISH:
                return PrefabID.Model_Character_Mafia_British;
            case ModelID.MAFIA_ITALIAN:
                return PrefabID.Model_Character_Mafia_Italian;
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

    public static ResourceRequest GetPrefab(ModelID ID)
    {
        return GetPrefab(GetPrefabID(ID));
    }

    public static ResourceRequest GetPrefab(WeaponID ID)
    {
        return GetPrefab(GetPrefabID(ID));
    }

    public static ResourceRequest GetPrefab(PrefabID ID)
    {
        string fileName = "Prefabs/" + ID.ToString();

        Debug.Log("Loading Prefab: " + fileName);

        ResourceRequest resourceRequest = Resources.LoadAsync<GameObject>(fileName);

        return resourceRequest;
    }

    //Every single prefab in the Resources folder must correspond to a unique ID
    public enum PrefabID
    {
        INVALID,

        //Character
        Character_Anchor_Overhead,
        Character_AudioSource_Female,
        Character_AudioSource_Male,
        Character_Decal,
        Character_Marker_Default,
        Character_Marker_Enemy,
        Character_Marker_Player,

        //Encounter
        Encounter_Camera_Character,
        Encounter_Camera_Main,
        Encounter_Environment_LoadingDock,
        Encounter_Manager_Audio,
        Encounter_Manager_CameraRig,
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
        Model_Character_DEA_Female,
        Model_Character_DEA_Male,
        Model_Character_Mafia_British,
        Model_Character_Mafia_Italian,
        Model_Weapon_Pistol,
        Model_Weapon_Revolver,
        Model_Weapon_SMG,

    }

    public static ResourceRequest GetTexture(TextureID ID)
    {
        string fileName = "Sprites/" + ID.ToString();

        Debug.Log("Loading Texture: " + fileName);

        ResourceRequest resourceRequest = Resources.LoadAsync<Texture2D>(fileName);

        return resourceRequest;
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
}
