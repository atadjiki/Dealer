using System;
using System.Collections.Generic;

namespace Constants
{
    public struct CharacterDefinition
    {
        public Game.CharacterID ID;

        public Game.ModelID[] AllowedModels;

        public Game.WeaponID[] AllowedWeapons;

        public int BaseHealth;

        //yep these are hardcoded :)
        public static CharacterDefinition Get(Game.CharacterID uniqueID)
        {
            switch (uniqueID)
            {
                case Game.CharacterID.PLAYER_1:
                {
                    return new CharacterDefinition()
                    {
                        ID = uniqueID,
                        AllowedModels = new Game.ModelID[]
                        {
                            Game.ModelID.DEA_MALE,
                        },
                        AllowedWeapons = new Game.WeaponID[]
                        {
                            Game.WeaponID.Revolver,
                        },

                        BaseHealth = 10,
                    };
                }
                case Game.CharacterID.PLAYER_2:
                {
                    return new CharacterDefinition()
                    {
                        ID = uniqueID,
                        AllowedModels = new Game.ModelID[]
                        {
                        Game.ModelID.DEA_FEMALE,
                        },
                        AllowedWeapons = new Game.WeaponID[]
                        {
                        Game.WeaponID.Revolver,
                        },

                        BaseHealth = 10,
                    };
                }
                case Game.CharacterID.HENCHMAN:
                {
                    return new CharacterDefinition()
                    {
                        ID = uniqueID,
                        AllowedModels = new Game.ModelID[]
                        {
                            Game.ModelID.MAFIA_BRITISH,
                            Game.ModelID.MAFIA_ITALIAN,
                        },
                            AllowedWeapons = new Game.WeaponID[]
                        {
                            Game.WeaponID.Pistol,
                            Game.WeaponID.Revolver,
                        },

                        BaseHealth = 4,
                    };
                }
                default:
                {
                    return new CharacterDefinition()
                    {
                        ID = uniqueID,
                        AllowedModels = new Game.ModelID[]
                        {
                            Game.ModelID.MAFIA_BRITISH,
                        },
                        AllowedWeapons = new Game.WeaponID[]
                        {
                        },

                        BaseHealth = 1,
                    };
                }
            }
        }
    }
}
