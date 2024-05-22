using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static partial class Constants
{
    //Environment
    public enum EnvironmentLayer
    {
        NONE,
        GROUND,
        OBSTACLE_HALF,
        OBSTACLE_FULL,
        WALL,
        CHARACTER
    };

    public enum EnvironmentDirection
    {
        NORTH,
        SOUTH,
        WEST,
        EAST,
        NORTH_EAST,
        NORTH_WEST,
        SOUTH_EAST,
        SOUTH_WEST
    }

    public enum EnvironmentCover
    {
        NONE,
        HALF,
        FULL
    };

    //Character

    public enum TeamID
    {
        NONE,
        PLAYER,
        ENEMY
    }

    public enum CharacterID
    {
        DEBUGBERT, //the default blue skeleton from Mixamo!
    }

    public enum ModelID
    {
        NONE,

        MALE_DEA,

        MALE_GOON,

        MALE_HENCHMAN,

        MALE_GENERIC,
    }

    public enum CharacterAnim
    {
        IDLE,
        MOVING
    }

    public enum AbilityID
    {
        NONE,
        MOVE_HALF,
        MOVE_FULL,
        FIRE_WEAPON,
    }

    //Encounter
    public enum EncounterState
    {
        NONE,

        INIT,

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

    public enum TargetType
    {
        Enemy,
        Ally,
        None,
    }
}
