namespace Constants
{
    public class CharacterConstants
    {
        public enum Mode { Stationary, Wander, Selected, None };

        public enum BehaviorType
        {
            MoveToLocation,
            MoveToRandomLocation,
            Idle,
            Approach,
            Interact,
            Sit,
            None
        };

        public enum UpdateState { Ready, Busy, None };

        public enum Team { NPC, Ally, Enemy };

        public enum CharacterID
        {
            Male_1,
            Female_1
        };

        public enum GenderType
        {
            Male,
            Female
        }

        public static GenderType GetGenderBYID(CharacterID ID)
        {
            switch(ID)
            {
                case CharacterID.Male_1:
                    return GenderType.Male;
                case CharacterID.Female_1:
                    return GenderType.Female;
            }

            return GenderType.Male;
        }
    }
}
