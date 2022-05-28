namespace Constants
{
    public class CharacterConstants
    {
        public enum CharacterState { None, WaitingForUpdate, Moving, Waiting, PerformingTask };

        public static string StateToString(CharacterState state)
        {
            switch(state)
            {
                case CharacterState.Moving:
                    return "Moving";
                case CharacterState.PerformingTask:
                    return "Performing Task";
                case CharacterState.Waiting:
                    return "Waiting";
                case CharacterState.WaitingForUpdate:
                    return "Waiting For Update";
                default:
                    return "";
            }
        }

        public enum Team { NPC, Ally, Enemy };

        public enum CharacterID
        {
            Male_1,
            Male_2,
            Male_3,
            Male_4,
            Female_1,
            Female_2,
            Female_3,
            Female_4
        };

        public enum GenderType
        {
            Male,
            Female
        }

        public static GenderType GetGenderBYID(CharacterID ID)
        {
            string idString = ID.ToString();

            if(idString.Contains(GenderType.Female.ToString()))
            {
                return GenderType.Female;
            }

            return GenderType.Male;
        }
    }
}
