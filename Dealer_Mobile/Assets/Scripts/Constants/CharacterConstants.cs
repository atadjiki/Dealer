namespace Constants
{
    public class CharacterConstants
    {
        public enum TeamID
        {
            Player,
            Enemy,
            Neutral,
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

        public enum CharacterID
        {
            NONE,

            //Enemy
            HENCHMAN,

            //Player
            PLAYER_1,
            PLAYER_2,
        }

        public enum EnemyID
        {
            //Enemy
            HENCHMAN,
        }

        public enum PlayerID
        {
            //Player
            PLAYER_1,
            PLAYER_2,
        }

        public enum AbilityID
        {
            Reload,
            Heal,
            SkipTurn,
            Attack,
        }

        public static TeamID GetTeamByID(CharacterConstants.CharacterID ID)
        {
            switch(ID)
            {
                case CharacterID.HENCHMAN:
                    return TeamID.Enemy;
                case CharacterID.PLAYER_1:
                case CharacterID.PLAYER_2:
                    return TeamID.Player;
                default:
                    return TeamID.Neutral;
            }
        }

        //helpers
        public static CharacterID ToCharacterID(EnemyID ID)
        {
            switch (ID)
            {
                case EnemyID.HENCHMAN:
                    return CharacterID.HENCHMAN;
            }

            return CharacterID.NONE;
        }

        public static CharacterID ToCharacterID(PlayerID ID)
        {
            switch (ID)
            {
                case PlayerID.PLAYER_1:
                    return CharacterID.PLAYER_1;
                case PlayerID.PLAYER_2:
                    return CharacterID.PLAYER_2;
            }

            return CharacterID.NONE;
        }
    }
}
