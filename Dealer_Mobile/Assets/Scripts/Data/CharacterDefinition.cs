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
                case CharacterConstants.CharacterID:
                {
                    return new CharacterDefinition()
                    {
                        ID = CharacterConstants.CharacterID.HENCHMAN,
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
            }
        }
    }
}
