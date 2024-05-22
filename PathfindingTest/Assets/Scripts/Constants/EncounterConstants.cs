using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static partial class Constants
{
    public static int GetAbilityCost(AbilityID ability)
    {
        switch (ability)
        {
            case AbilityID.MOVE_HALF:
                return 1;
            default:
                return 2;
        }
    }

    public static TargetType GetTargetType(AbilityID abilityID)
    {
        switch (abilityID)
        {
            case AbilityID.FIRE_WEAPON:
                return TargetType.Enemy;
            default:
                return TargetType.None;
        }
    }

    //public static TeamID GetOpposingTeam(CharacterComponent characterComponent)
    //{
    //    return GetOpposingTeam(GetTeamByID(characterComponent.GetID()));
    //}

    public static TeamID GetOpposingTeam(TeamID team)
    {
        if (team == TeamID.PLAYER)
        {
            return TeamID.ENEMY;
        }
        else if (team == TeamID.ENEMY)
        {
            return TeamID.PLAYER;
        }

        return TeamID.NONE;
    }
}
