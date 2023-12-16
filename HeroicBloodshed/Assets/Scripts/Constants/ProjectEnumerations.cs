using System.Collections.Generic;
using UnityEngine;

public static partial class Constants
{
    public enum TeamID
    {
        None,
        Player,
        Enemy,
    }

    public enum CharacterID
    {
        NONE,

        //Enemy
        HENCHMAN,
        GOON,

        //Player
        PLAYER_1,
        PLAYER_2,
    }

    public enum GenderID
    {
        Male,
        Female,
    }

    public enum ModelID
    {
        NONE,

        MALE_DEA,

        MALE_GENERIC,

        MALE_GENERIC_ENEMY,
    }

    public enum PlayerID
    {
        NONE,

        //Player
        PLAYER_1,
        PLAYER_2,
    }

    public enum EnemyID
    {
        NONE,

        //Enemy
        HENCHMAN,
        GOON,
    }

    public enum AbilityID
    {
        NONE,

        MoveHalf,
        MoveFull,
        Attack,
        Reload,
        Heal,
        SkipTurn,
    }

    public enum TargetType
    {
        Enemy,
        Ally,
        None,
    }

    public enum WeaponID
    {
        None,
        Revolver,
        Pistol,
        SMG,
    }

    public enum CharacterAudioType
    {
        Confirm,
        Await,
        Death
    };

    public enum CharacterEvent
    {
        ABILITY,

        SELECTED,

        DESELECTED,

        TARGETED,

        UNTARGETED,

        DAMAGE, //triggers a hit or kill event 

        HIT,

        KILLED,

        MOVING,

        STOPPED,
    }

    public enum EncounterState
    {
        INIT,

        INTRO,

        SETUP_COMPLETE,//waiting for encounter to begin

        BUILD_QUEUES,//build queues to see which characters have actions this turn

        CHECK_CONDITIONS,//check queues to see if the encounter can be declared Done

        DONE,//the encounter is done and we can exit

        TEAM_UPDATED,

        SELECT_CURRENT_CHARACTER,//select the next character in the current team queue

        CHOOSE_ACTION,

        CHOOSE_TARGET,

        CANCEL_ACTION,

        PERFORM_ACTION,//perform the chosen action

        DESELECT_CURRENT_CHARACTER,//deselect current character and pop them from their team's queue

        UPDATE //update the turn count, etc 
    }

    public enum AnimState
    {
        Idle,
        Walking,
        Running,
        Reload,
        Attack_Single,
        Hit_Light,
        Hit_Medium,
        Hit_Heavy,
        Heal,
        SkipTurn,
        Interact,
        Dead,
    }

    public enum AnimID
    {
        //for cutscenes/non combat use

        //locomotion 
        Anim_Character_Idle,
        Anim_Character_Walking,
        Anim_Character_Running,

        //pistol/handgun
        Anim_Character_Pistol_Idle,
        Anim_Character_Pistol_Walking,
        Anim_Character_Pistol_Running,
        Anim_Character_Pistol_Attack_Single,
        Anim_Character_Pistol_Hit_Light,
        Anim_Character_Pistol_Hit_Medium,
        Anim_Character_Pistol_Hit_Heavy,
        Anim_Character_Pistol_Reload,

        //Gestures
        Anim_Character_ButtonPush,
        Anim_Character_ShoulderRub,
        Anim_Character_WipingSweat,

        //death
        Anim_Character_Death
    }

    public enum EnvironmentNodeTagType
    {
        Ground, 
        Obstacle,
        SpawnLocation,
        Character
    }

    public enum EnvironmentObstacleType
    {
        NoCover,
        HalfCover,
        FullCover,
    }

    public enum MovementRangeType
    {
        None,
        Half,
        Full
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

        EnvironmentCoverDecal,

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