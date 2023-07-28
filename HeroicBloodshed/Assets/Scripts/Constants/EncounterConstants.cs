public static partial class Constants
{
    public enum EncounterState
    {
        NONE,
        SETUP, //performing setup 
        READY, //encounter is all setup and ready to begin 
        WAITING_FOR_PLAYER, //waiting on player input
        BUSY, //performing actions for player or AI
        COMPLETE,//encouter is finished
    }
}