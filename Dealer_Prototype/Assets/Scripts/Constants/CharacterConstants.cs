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
            None
        };

        public enum UpdateState { Ready, Busy, None };

        public enum Team { NPC, Ally, Enemy };

        public enum CharacterID
        {
            Male_1,
            Female_1
        };
    }
}
