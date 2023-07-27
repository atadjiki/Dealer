using System;
using System.Collections.Generic;
using static Constants;

public struct CharacterDefinition
{
    public CharacterID ID;

    public ModelID[] AllowedModels;

    public WeaponID[] AllowedWeapons;

    public int BaseHealth;

    //yep these are hardcoded :)
    public static CharacterDefinition Get(CharacterID uniqueID)
    {
        switch (uniqueID)
        {
            case CharacterID.PLAYER_1:
                {
                    return new CharacterDefinition()
                    {
                        ID = uniqueID,
                        AllowedModels = new ModelID[]
                        {
                            ModelID.DEA_MALE,
                        },
                        AllowedWeapons = new WeaponID[]
                        {
                            WeaponID.Revolver,
                        },

                        BaseHealth = 10,
                    };
                }
            case CharacterID.PLAYER_2:
                {
                    return new CharacterDefinition()
                    {
                        ID = uniqueID,
                        AllowedModels = new ModelID[]
                        {
                        ModelID.DEA_FEMALE,
                        },
                        AllowedWeapons = new WeaponID[]
                        {
                        WeaponID.Revolver,
                        },

                        BaseHealth = 10,
                    };
                }
            case CharacterID.HENCHMAN:
                {
                    return new CharacterDefinition()
                    {
                        ID = uniqueID,
                        AllowedModels = new ModelID[]
                        {
                            ModelID.MAFIA_BRITISH,
                            ModelID.MAFIA_ITALIAN,
                        },
                        AllowedWeapons = new WeaponID[]
                        {
                            WeaponID.Pistol,
                            WeaponID.Revolver,
                        },

                        BaseHealth = 4,
                    };
                }
            default:
                {
                    return new CharacterDefinition()
                    {
                        ID = uniqueID,
                        AllowedModels = new ModelID[]
                        {
                            ModelID.MAFIA_BRITISH,
                        },
                        AllowedWeapons = new WeaponID[]
                        {
                        },

                        BaseHealth = 1,
                    };
                }
        }
    }
}
