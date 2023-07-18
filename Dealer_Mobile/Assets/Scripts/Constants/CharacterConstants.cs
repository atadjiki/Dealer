namespace Constants
{
    public class CharacterConstants
    {
        public enum TeamID
        {
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

        public enum UniqueID
        {
            //Enemy
            HENCHMAN,
        }

        public enum AbilityID
        {
            Reload,
            Heal,
            SkipTurn,
            Attack,
        }
    }
}
