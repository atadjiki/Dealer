using System;
using System.Collections.Generic;

namespace Constants
{
    public struct CharacterDefinition
    {
        public CharacterConstants.UniqueID ID;

        public CharacterConstants.ModelID[] AllowedModels;

        public CharacterConstants.WeaponID[] AllowedWeapons;

        public int BaseHealth;

        //yep these are hardcoded :)
        public static CharacterDefinition GetCharacterDefinition(CharacterConstants.UniqueID uniqueID)
        {
            switch (uniqueID)
            {
                case CharacterConstants.UniqueID:
                {
                    return new CharacterDefinition()
                    {
                        ID = CharacterConstants.UniqueID.HENCHMAN,
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
