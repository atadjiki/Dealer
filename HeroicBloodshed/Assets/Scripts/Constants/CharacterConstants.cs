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

    public static string GetDisplayString(CharacterID characterID)
    {
        switch(characterID)
        {
            case CharacterID.HENCHMAN:
                return "Henchman";
            case CharacterID.PLAYER_1:
                return "Mulder";
            case CharacterID.PLAYER_2:
                return "Scully";
            default:
                return characterID.ToString();
        }
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
        NONE,

        Reload,
        Heal,
        SkipTurn,
        Attack,
    }

    public static string GetDisplayString(AbilityID abilityID)
    {
        switch(abilityID)
        {
            case AbilityID.Attack:
                return "Attack";
            case AbilityID.Heal:
                return "Heal";
            case AbilityID.Reload:
                return "Reload";
            case AbilityID.SkipTurn:
                return "Skip Turn";
            default:
                return abilityID.ToString();
        }
    }

    public static List<AbilityID> GetAllowedAbilities(CharacterID characterID)
    {
        List<AbilityID> CharacterAbilities = new List<AbilityID>()
        {
            AbilityID.SkipTurn, //all characters have this by default
            AbilityID.Attack
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
                return TeamID.Player;
            case CharacterID.PLAYER_2:
                return TeamID.Player;
        }

        return TeamID.None;
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
                return Color.blue;
            case TeamID.Enemy:
                return Color.red;
            default:
                return Color.clear;
        }
    }

    public static TeamID GetOpposingTeam(CharacterComponent characterComponent)
    {
        return GetOpposingTeam(GetTeamByID(characterComponent.GetID()));
    }

    public static TeamID GetOpposingTeam(TeamID team)
    {
        if(team == TeamID.Player)
        {
            return TeamID.Enemy;
        }
        else if(team == TeamID.Enemy)
        {
            return TeamID.Player;
        }

        return TeamID.None;
    }
}
