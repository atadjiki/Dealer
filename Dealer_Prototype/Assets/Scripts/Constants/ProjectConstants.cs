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

        public enum SceneName 
        { 
            //root
            Scene_StaticManagers, 
            Scene_CameraRig, 
            //ui
            Scene_UI_GamePlay,
            Scene_UI_Loading,
            Scene_UI_Pause,
            //environments
            Scene_Environment_Safehouse, 
            //
            Null 
        }

        public static Enumerations.SceneName GetSceneNameFromGameMode(Enumerations.GameMode gameMode)
        {
            if(gameMode == GameMode.GamePlay)
            {
                return SceneName.Scene_UI_GamePlay;
            }
            else if(gameMode == GameMode.Loading)
            {
                return SceneName.Scene_UI_Loading;
            }
            else if(gameMode == GameMode.Paused)
            {
                return SceneName.Scene_UI_Pause;
            }

            return SceneName.Null;
        }

        public static Enumerations.SceneName GetSceneNameFromGameplayState(Enumerations.GamePlayState gameplayState)
        {
            if (gameplayState == Enumerations.GamePlayState.Safehouse)
            {
                return Enumerations.SceneName.Scene_Environment_Safehouse;
            }

            return Enumerations.SceneName.Null;
        }

        public static Enumerations.SceneName GetSceneNameFromScene(Scene scene)
        {
            if (scene.name == Enumerations.SceneName.Scene_Environment_Safehouse.ToString())
            {
                return Enumerations.SceneName.Scene_Environment_Safehouse;
            }
            else if(scene.name == Enumerations.SceneName.Scene_UI_GamePlay.ToString())
            {
                return SceneName.Scene_UI_GamePlay;
            }
            else if (scene.name == Enumerations.SceneName.Scene_UI_Loading.ToString())
            {
                return SceneName.Scene_UI_Loading;
            }
            else if (scene.name == Enumerations.SceneName.Scene_UI_Pause.ToString())
            {
                return SceneName.Scene_UI_Pause;
            }

            return Enumerations.SceneName.Null;
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