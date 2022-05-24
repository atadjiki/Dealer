namespace Constants
{
    public class TaskConstants
    {
        public enum TaskState { WaitingForUpdate, Idle, Busy, None };
        public enum TaskType { Process, Sell, Pickup, None };
    }
}