using System;
using UnityEngine;
using static Constants;

[Serializable]
public struct CharacterStateData
{
    public AbilityID ActiveAbility;
    public CharacterComponent ActiveTarget;
    public Vector3 ActiveDestination;

    public int Health;
    public int ActionPoints;

    private TeamID NativeTeam;
    public TeamID CurrentTeam;

    public bool IsAlive()
    {
        return Health > 0;
    }

    public bool IsDead()
    {
        return Health <= 0;
    }

    public bool HasActionPoints()
    {
        return ActionPoints > 0;
    }

    public bool CanAffordAbility(AbilityID ability)
    {
        int cost = GetAbilityCost(ability);

        return (cost <= ActionPoints);
    }

    public void DerementActionPoints(AbilityID ability)
    {
        int cost = GetAbilityCost(ability);

        ActionPoints -= cost;
    }

    public void AdjustHealth(int amount)
    {
        Health += amount;
    }

    public void ResetForTurn(CharacterID ID)
    {
        CharacterDefinition def = ResourceUtil.GetCharacterDefinition(ID);

        ActiveAbility = AbilityID.NONE;
        ActiveTarget = null;
        ActiveDestination = Vector3.zero;

        ActionPoints = def.BaseActionPoints;
    }

    public string GetInfoString()
    {
        return
            "HP: " + Health + "\n" +
            "AP: " + ActionPoints + "\n";

    }

    public static CharacterStateData Build(CharacterID ID)
    {
        CharacterDefinition def = ResourceUtil.GetCharacterDefinition(ID);

        return new CharacterStateData()
        {
            ActiveAbility = AbilityID.NONE,
            ActiveTarget = null,
            ActiveDestination = Vector3.zero,
            Health = def.BaseHealth,
            ActionPoints = def.BaseActionPoints,

            NativeTeam = GetNativeTeamByID(ID),
            CurrentTeam = GetNativeTeamByID(ID),
        };
    }

    public void ResetTeam()
    {
        CurrentTeam = NativeTeam;
    }
}