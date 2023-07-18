namespace Constants
{
    public class CharacterConstants
    {
        public enum TeamID
        {
            NONE,
            Player,
            Enemy,
        }

        public enum WeaponID
        {
            Revolver,
            Pistol,
        }

        public enum ModelID
        {
            //dea
            DEA_MALE,
            DEA_FEMALE,

            //mafia
            MAFIA_BRITISH,
            MAFIA_ITALIAN,

            NONE,
        }

        public enum ClassID
        {
            GRUNT,
        }

        public enum TypeID
        {
            Default,
            Alternate
        }

        public static ModelID GetModelID(ClassID classID, TypeID type, TeamID team)
        {
            switch(team)
            {
                case TeamID.Player:
                    return GetModelID_DEA(classID, type);
                case TeamID.Enemy:
                    return GetModelID_Mafia(classID, type);
                default:
                    return ModelID.NONE;
            }
        }

        private static ModelID GetModelID_DEA(ClassID classID, TypeID type)
        {
            switch(classID)
            {
                case ClassID.GRUNT:
                {
                    if(type == default) { return ModelID.DEA_MALE; }
                    else { return ModelID.DEA_FEMALE; }
                }

                default:
                    return ModelID.DEA_MALE;
            }
        }

        private static ModelID GetModelID_Mafia(ClassID classID, TypeID type)
        {
            switch (classID)
            {
                case ClassID.GRUNT:
                    {
                        if (type == default) { return ModelID.MAFIA_BRITISH; }
                        else { return ModelID.MAFIA_ITALIAN; }
                    }

                default:
                    return ModelID.DEA_MALE;
            }
        }

        public static WeaponID GetWeapon(ClassID ID, TeamID Team)
        {
            switch(Team)
            {
                case TeamID.Player:
                    return GetWeapon_DEA(ID);
                case TeamID.Enemy:
                    return GetWeapon_Mafia(ID);
                default:
                    return WeaponID.Revolver;
            }
        }

        private static WeaponID GetWeapon_DEA(ClassID ID)
        {
            switch(ID)
            {
                case ClassID.GRUNT:
                    return WeaponID.Revolver;
                default:
                    return WeaponID.Revolver;
            }
        }

        private static WeaponID GetWeapon_Mafia(ClassID ID)
        {
            switch (ID)
            {
                case ClassID.GRUNT:
                    return WeaponID.Pistol;
                default:
                    return WeaponID.Pistol;
            }
        }
    }
}
