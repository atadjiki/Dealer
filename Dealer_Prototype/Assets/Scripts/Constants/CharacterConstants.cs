namespace Constants
{
    public class CharacterConstants
    {
        public enum Mode { Stationary, Wander, Selected, None };
        public enum Behavior { MoveToRandomLocation, FindInteractable };
        public enum ActionType { Move, Idle, None };

        public enum UpdateState { Ready, Busy, None };
        public enum State { Idle, Moving, Interacting, Talking, Sitting, Unavailable };


        public enum Team { NPC, Ally, Enemy };

        public enum CharacterID
        {
            Male_1,
            Female_1
        };
    }
}
