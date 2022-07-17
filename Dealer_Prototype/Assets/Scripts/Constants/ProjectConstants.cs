namespace Constants
{
    public class Enumerations
    {
        public enum GameMode { GamePlay, Paused, Loading };
        public enum GamePlayState { Safehouse, Map, Inventory, Ledger, Inactive }
    }
    
    public class SaveKeys
    {
        public const string Player_Name = "Player_Name";
        public const string Player_Money = "Player_Money";
        public const string Player_Drugs = "Player_Drugs";

        public const string Game_Day = "Game_Day";
    }
}