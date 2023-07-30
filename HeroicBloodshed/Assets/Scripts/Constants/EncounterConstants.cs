public static partial class Constants
{
    public enum EncounterStateID
    {
        NONE, //the state should never be None :)
        SETUP,
        BUSY, //we are waiting on setup
        WAITING_ON_INPUT, //waiting on player to select an action for their character
        WAITING_ON_AI, //waiting on computer to select an action 
        PERFORMING_ACTION, //block input while character is performing their action 
        DONE, //the encounter is done and we are ready to advance 
    }
}