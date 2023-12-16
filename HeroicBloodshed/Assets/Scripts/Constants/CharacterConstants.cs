using System.Collections.Generic;
using UnityEngine;

public static partial class Constants
{
    public static int GetAbilityCost(AbilityID ability)
    {
        switch(ability)
        {
            case AbilityID.MoveHalf:
                return 1;
            case AbilityID.Reload:
                return 1;
            default:
                return 2;
        }
    }

    public static TargetType GetTargetType(AbilityID abilityID)
    {
        switch(abilityID)
        {
            case AbilityID.Attack:
                return TargetType.Enemy;
            default:
                return TargetType.None;
        }
    }

    public static List<AbilityID> GetAllowedAbilities(CharacterID characterID)
    {
        List<AbilityID> CharacterAbilities = new List<AbilityID>()
        {
            AbilityID.MoveHalf,
            AbilityID.MoveFull,
            AbilityID.Attack,
            AbilityID.SkipTurn, //all characters have this by default
            AbilityID.Reload,
        };

        //eventually we will add dynamically abilities based on weapon type and team 
        if (GetTeamByID(characterID)== TeamID.Player)
        {
            CharacterAbilities.Add(AbilityID.Heal);
        }

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

            return EncounterManager.Instance.AreTargetsAvailable();
        }
        else if(abilityID == AbilityID.Reload)
        {
            if(characterComponent.IsAmmoFull())
            {
                return false;
            }
        }
        else if(abilityID == AbilityID.Heal)
        {
            if(characterComponent.IsHealthFull())
            {
                return false;
            }
        }

        return true;
    }

    public static GenderID GetGender(CharacterID ID)
    {
        switch(ID)
        {
            case CharacterID.PLAYER_2:
                return GenderID.Female;
            default:
                return GenderID.Male;
        }
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
