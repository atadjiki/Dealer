namespace Constants
{
    public class CharacterConstants
    {
        public enum Behavior { Stationary, Wander, None };
        public enum StationType { Conversation, Bar, Leaning, None };
        public enum UpdateState { Ready, Busy, None };
        public enum State { Idle, Moving, Interacting, Talking, Unavailable, Sitting };
        public enum ActionType { Move, Idle, None };

        public enum CharacterID
        {
            Male_1,
            Female_1
        };
    }
}
