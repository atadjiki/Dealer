public static partial class Constants
{
    public enum EncounterState
    {
        SETUP,//waiting for encounter to begin

        BUILD_QUEUES,//build queues to see which characters have actions this turn

        CHECK_CONDITIONS,//check queues to see if the encounter can be declared Done

        DONE,//the encounter is done and we can exit

        PROCESS_TURN,//see which team queue to process
         
        SELECT_CURRENT_CHARACTER,//select the next character in the current team queue

        WAIT_FOR_PLAYER_INPUT,//if player turn, wait for input

        CHOOSE_AI_ACTION,//if AIs turn, choose an action for them 

        PERFORM_ACTION,//perform the chosen action

        DESELECT_CURRENT_CHARACTER,//deselect current character and pop them from their team's queue

        UPDATE //update the turn count, etc 
    }
}