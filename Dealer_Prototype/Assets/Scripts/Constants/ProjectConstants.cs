using UnityEngine.SceneManagement;

namespace Constants
{
    public class Enumerations
    {
        public enum GameMode { Root, GamePlay, Paused, Loading };
        public enum GamePlayState { Safehouse, Map, Inventory, Ledger, Inactive }

        public enum SceneType
        {
            Root,
            Environment,
            UI,
            Null
        }

        public static bool AllowSceneActivation(Enumerations.SceneType type)
        {
            switch (type)
            {
                case Enumerations.SceneType.Environment:
                    return false;
                default:
                    return true;
            }
        }

        public static bool RequiresLoadingScreen(Enumerations.SceneType type)
        {
            switch (type)
            {
                case SceneType.Environment:
                    return true;
                default:
                    return false;
            }
        }
    }

    public class SceneName
    {
        //root
        public const string StaticManagers = "Scene_StaticManagers";
        public const string CameraRig = "Scene_CameraRig";
        //ui
        public const string UI_GamePlay = "Scene_UI_GamePlay";
        public const string UI_Loading = "Scene_UI_Loading";
        public const string UI_Pause = "Scene_UI_Pause";
        //environments
        public const string Environment_Safehouse = "Scene_Environment_Safehouse";

        public static string GetSceneNameFromGameMode(Enumerations.GameMode gameMode)
        {
            if (gameMode == Enumerations.GameMode.GamePlay)
            {
                return UI_GamePlay;
            }
            else if (gameMode == Enumerations.GameMode.Loading)
            {
                return UI_Loading;
            }
            else if (gameMode == Enumerations.GameMode.Paused)
            {
                return UI_Pause;
            }

            return null;
        }

        public static string GetSceneNameFromGameplayState(Enumerations.GamePlayState gameplayState)
        {
            if (gameplayState == Enumerations.GamePlayState.Safehouse)
            {
                return Environment_Safehouse;
            }

            return null;
        }
    }

    public class SaveKeys
    {
        public const string Player_Name = "Player_Name";
        public const string Player_Money = "Player_Money";
        public const string Player_Drugs = "Player_Drugs";

        public const string Game_Day = "Game_Day";
    }
}