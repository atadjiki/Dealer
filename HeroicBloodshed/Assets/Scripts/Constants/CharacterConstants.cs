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

    public static string GetDisplayString(WeaponID weapon)
    {
        switch(weapon)
        {
            case WeaponID.Pistol:
                return "Pistol";
            case WeaponID.Revolver:
                return "Revolver";
            default:
                return weapon.ToString();
        }
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
        GOON,

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
            case CharacterID.GOON:
                return "Goon";
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
        GOON,
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

    public enum TargetType
    {
        Enemy,
        Ally,
        None,
    }

    public static TargetType GetTargetType(AbilityID abilityID)
    {
        switch(abilityID)
        {
            case AbilityID.Attack:
                return TargetType.Enemy;
            case AbilityID.Heal:
                return TargetType.Ally;
            case AbilityID.Reload:
                return TargetType.None;
            case AbilityID.SkipTurn:
                return TargetType.None;
            default:
                return TargetType.None;
        }
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

    public static string GetEventString(AbilityID abilityID)
    {
        switch (abilityID)
        {
            case AbilityID.Attack:
                return "Attacking";
            case AbilityID.Heal:
                return "Healing";
            case AbilityID.Reload:
                return "Reloading";
            case AbilityID.SkipTurn:
                return "Skipping Turn";
            default:
                return abilityID.ToString();
        }
    }

    public static List<AbilityID> GetAllowedAbilities(CharacterID characterID)
    {
        List<AbilityID> CharacterAbilities = new List<AbilityID>()
        {
            AbilityID.Attack,
            AbilityID.SkipTurn, //all characters have this by default
            AbilityID.Reload,
        };

        //eventually we will add dynamically abilities based on weapon type and team 

        return CharacterAbilities;
    }

    public static bool ValidateAbility(AbilityID abilityID, CharacterComponent characterComponent)
    {
        if(!GetAllowedAbilities(characterComponent.GetID()).Contains(abilityID))
        {
            return false;
        }

        if(abilityID == AbilityID.Attack)
        {
            if(characterComponent.GetRemainingAmmo() == 0)
            {
                return false;
            }
        }
        else if(abilityID == AbilityID.Reload)
        {
            if(characterComponent.IsAmmoFull())
            {
                return false;
            }
        }

        return true;
    }

    public static TeamID GetTeamByID(CharacterID ID)
    {
        switch (ID)
        {
            case CharacterID.GOON:
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
            case EnemyID.GOON:
                return CharacterID.GOON;
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

    public static Color GetColorByTeam(TeamID teamID, float opacity)
    {
        Color teamColor;

        switch(teamID)
        {
            case TeamID.Player:
                teamColor = Color.blue;
                break;
            case TeamID.Enemy:
                teamColor = Color.red;
                break;
            default:
                teamColor = Color.clear;
                break;
        }

        teamColor.a = opacity;

        return teamColor;
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
