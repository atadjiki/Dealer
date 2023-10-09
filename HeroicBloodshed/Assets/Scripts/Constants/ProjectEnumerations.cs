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

        //combat

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

    public enum EncounterGridTileType
    {
        Empty,
        NoCover,
        HalfCover,
        FullCover,
    }

    public enum SafehouseMenuID
    {
        None,
        Inventory,
        Map
    }

    public enum OwnerID
    {
        Player_Stash,
        Player_Bag
    }

    public enum InventoryItemID
    {
        //DRUGS

        //None,
        //Heroin,
        Cocaine,
        //Ecstasy,
        //LSD,
        //Mushrooms,
        //Amphetamines
    }
}