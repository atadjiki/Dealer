namespace Constants
{
    public class Enumerations
    {
        public enum GameMode { GamePlay, Paused, Loading };
        public enum GamePlayState { Safehouse, Map, Inventory, Ledger, Inactive }
    }
    
    public class SaveKeys
    {
        public const string Player_Name = "PlayerName";
        public const string Player_Money = "PlayerMoney";
    }
}