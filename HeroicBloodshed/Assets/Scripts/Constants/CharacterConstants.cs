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

    public enum WeaponID
    {
        None,
        Revolver,
        Pistol,
    }

    public enum ModelID
    {
        NONE,

        //dea
        DEA_MALE,
        DEA_FEMALE,

        //mafia
        MAFIA_BRITISH,
        MAFIA_ITALIAN,
    }

    public enum CharacterID
    {
        NONE,

        //Enemy
        HENCHMAN,

        //Player
        PLAYER_1,
        PLAYER_2,
    }

    public enum EnemyID
    {
        NONE,

        //Enemy
        HENCHMAN,
    }

    public enum PlayerID
    {
        NONE,

        //Player
        PLAYER_1,
        PLAYER_2,
    }

    public enum AbilityID
    {
        None,
        Reload,
        Heal,
        SkipTurn,
        Attack,
    }

    public static List<AbilityID> GetAllowedAbilities(CharacterID characterID)
    {
        List<AbilityID> CharacterAbilities = new List<AbilityID>()
        {
            AbilityID.SkipTurn //all characters have this by default
        };

        //eventually we will add dynamically abilities based on weapon type and team 

        return CharacterAbilities;
    }

    public static TeamID GetTeamByID(CharacterID ID)
    {
        switch (ID)
        {
            case CharacterID.HENCHMAN:
                return TeamID.Enemy;
            case CharacterID.PLAYER_1:
            case CharacterID.PLAYER_2:
                return TeamID.Player;
        }

        return TeamID.Enemy;
    }

    //helpers
    public static CharacterID ToCharacterID(EnemyID ID)
    {
        switch (ID)
        {
            case EnemyID.HENCHMAN:
                return CharacterID.HENCHMAN;
        }

        return CharacterID.NONE;
    }

    public static CharacterID ToCharacterID(PlayerID ID)
    {
        switch (ID)
        {
            case PlayerID.PLAYER_1:
                return CharacterID.PLAYER_1;
            case PlayerID.PLAYER_2:
                return CharacterID.PLAYER_2;
        }

        return CharacterID.NONE;
    }

    public static Color GetColorByTeam(TeamID teamID)
    {
        switch(teamID)
        {
            case TeamID.Player:
                return Color.white;
            case TeamID.Enemy:
                return Color.red;
            default:
                return Color.clear;
        }
    }
}
