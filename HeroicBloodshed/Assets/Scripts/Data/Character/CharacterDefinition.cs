using System;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

[Serializable]
public struct CharacterDefinition
{
    public CharacterID ID;

    public ModelID[] AllowedModels;

    public WeaponID[] AllowedWeapons;

    public int BaseHealth;

    public int BaseActionPoints;

    public int CritChance;

    public bool RollCritChance()
    {
        float roll = UnityEngine.Random.Range(0, 99);

        if(CritChance > roll)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

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
                            ModelID.MALE_DEA,
                        },
                        AllowedWeapons = new WeaponID[]
                        {
                            WeaponID.Revolver,
                        },

                        BaseHealth = 16,
                        BaseActionPoints = 2,
                        CritChance = 30,
                    };
                }
            case CharacterID.PLAYER_2:
                {
                    return new CharacterDefinition()
                    {
                        ID = uniqueID,
                        AllowedModels = new ModelID[]
                        {
                        ModelID.MALE_DEA,
                        },
                        AllowedWeapons = new WeaponID[]
                        {
                        WeaponID.Revolver,
                        },

                        BaseHealth = 16,
                        BaseActionPoints = 2,
                        CritChance = 30,
                    };
                }
            case CharacterID.HENCHMAN:
                {
                    return new CharacterDefinition()
                    {
                        ID = uniqueID,
                        AllowedModels = new ModelID[]
                        {
                            ModelID.MALE_GENERIC_ENEMY,
                        },
                        AllowedWeapons = new WeaponID[]
                        {
                            WeaponID.Pistol,
                            WeaponID.Revolver,
                        },

                        BaseHealth = 4,
                        BaseActionPoints = 2,
                        CritChance = 0,
                    };
                }

            case CharacterID.GOON:
                {
                    return new CharacterDefinition()
                    {
                        ID = uniqueID,
                        AllowedModels = new ModelID[]
                        {
                            ModelID.MALE_GENERIC_ENEMY,
                        },
                        AllowedWeapons = new WeaponID[]
                        {
                            WeaponID.SMG,
                        },

                        BaseHealth = 7,
                        BaseActionPoints = 2,
                        CritChance = 0,
                    };
                }
            default:
                {
                    return new CharacterDefinition()
                    {
                        ID = uniqueID,
                        AllowedModels = new ModelID[]
                        {
                            ModelID.MALE_GENERIC,
                        },
                        AllowedWeapons = new WeaponID[]
                        {
                        },

                        BaseHealth = 1,
                        BaseActionPoints = 1,
                        CritChance = 0,
                    };
                }
        }
    }
}
