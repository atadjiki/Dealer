using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

[Serializable]
public struct CharacterAbilityInfo
{
    public int TotalUses;
    public int Charges;
}

public class CharacterAbilityTable : Dictionary<AbilityID, CharacterAbilityInfo>
{
    private CharacterID _ID;

    public CharacterAbilityTable(CharacterID ID)
    {
        _ID = ID;

        foreach(AbilityID abilityID in GetAllowedAbilities(ID))
        {
            CharacterAbilityInfo info = new CharacterAbilityInfo()
            {
                TotalUses = 0,
                Charges = 0,
            };


            Add(abilityID, info);
        }
    }
}
