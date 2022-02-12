namespace Constants
{
    public class AIConstants
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

        //schedule stuff
        public enum ScheduleTaskID
        {
            AcquireBeverage,
            TurnOnTelevision,
            TurnOffTelevision,
            WatchTelevision,
            SitOnChair,
            None
        };
    }
}
