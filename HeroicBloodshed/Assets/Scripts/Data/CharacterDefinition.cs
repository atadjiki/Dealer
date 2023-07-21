using System;
using System.Collections.Generic;

namespace Constants
{
    public struct CharacterDefinition
    {
        public CharacterConstants.CharacterID ID;

        public CharacterConstants.ModelID[] AllowedModels;

        public CharacterConstants.WeaponID[] AllowedWeapons;

        public int BaseHealth;

        //yep these are hardcoded :)
        public static CharacterDefinition GetCharacterDefinition(CharacterConstants.CharacterID uniqueID)
        {
            switch (uniqueID)
            {
                case CharacterConstants.CharacterID.PLAYER_1:
                {
                    return new CharacterDefinition()
                    {
                        ID = uniqueID,
                        AllowedModels = new CharacterConstants.ModelID[]
                        {
                            CharacterConstants.ModelID.DEA_MALE,
                        },
                        AllowedWeapons = new CharacterConstants.WeaponID[]
                        {
                            CharacterConstants.WeaponID.Revolver,
                        },

                        BaseHealth = 10,
                    };
                }
                case CharacterConstants.CharacterID.PLAYER_2:
                {
                    return new CharacterDefinition()
                    {
                        ID = uniqueID,
                        AllowedModels = new CharacterConstants.ModelID[]
                        {
                        CharacterConstants.ModelID.DEA_FEMALE,
                        },
                        AllowedWeapons = new CharacterConstants.WeaponID[]
                        {
                        CharacterConstants.WeaponID.Revolver,
                        },

                        BaseHealth = 10,
                    };
                }
                case CharacterConstants.CharacterID.HENCHMAN:
                {
                    return new CharacterDefinition()
                    {
                        ID = uniqueID,
                        AllowedModels = new CharacterConstants.ModelID[]
                        {
                            CharacterConstants.ModelID.MAFIA_BRITISH,
                            CharacterConstants.ModelID.MAFIA_ITALIAN,
                        },
                            AllowedWeapons = new CharacterConstants.WeaponID[]
                        {
                            CharacterConstants.WeaponID.Pistol,
                            CharacterConstants.WeaponID.Revolver,
                        },

                        BaseHealth = 4,
                    };
                }
                default:
                {
                    return new CharacterDefinition()
                    {
                        ID = uniqueID,
                        AllowedModels = new CharacterConstants.ModelID[]
                        {
                            CharacterConstants.ModelID.MAFIA_BRITISH,
                        },
                        AllowedWeapons = new CharacterConstants.WeaponID[]
                        {
                        },

                        BaseHealth = 1,
                    };
                }
            }
        }
    }
}
