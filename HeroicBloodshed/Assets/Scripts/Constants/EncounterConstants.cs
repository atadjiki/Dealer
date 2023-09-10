public static partial class Constants
{
    public enum EncounterEvent
    {
        CHARACTER_KILLED,
    }

    public enum EncounterState
    {
        INIT,

        SETUP_COMPLETE,//waiting for encounter to begin

        BUILD_QUEUES,//build queues to see which characters have actions this turn

        CHECK_CONDITIONS,//check queues to see if the encounter can be declared Done

        DONE,//the encounter is done and we can exit
         
        SELECT_CURRENT_CHARACTER,//select the next character in the current team queue

        CHOOSE_ACTION,

        PERFORM_ACTION,//perform the chosen action

        DESELECT_CURRENT_CHARACTER,//deselect current character and pop them from their team's queue

        UPDATE //update the turn count, etc 
    }

    public static string GetDisplayString(EncounterState state)
    {
        switch(state)
        {
            case EncounterState.INIT:
                return "Initializing...";
            case EncounterState.SETUP_COMPLETE:
                return "Setup Complete";
            case EncounterState.BUILD_QUEUES:
                return "Building Queues...";
            case EncounterState.CHECK_CONDITIONS:
                return "Checking Conditions...";
            case EncounterState.DONE:
                return "Done";
            case EncounterState.SELECT_CURRENT_CHARACTER:
                return "Selecting Next Character...";
            case EncounterState.CHOOSE_ACTION:
                return "Choosing action...";
            case EncounterState.PERFORM_ACTION:
                return "Performing Action...";
            case EncounterState.DESELECT_CURRENT_CHARACTER:
                return "Deselecting Current Character...";
            case EncounterState.UPDATE:
                return "Updating...";
            default:
                return state.ToString();

        }
    }
}