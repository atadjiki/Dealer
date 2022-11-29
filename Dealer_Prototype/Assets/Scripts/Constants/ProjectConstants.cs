using UnityEngine;
using UnityEngine.SceneManagement;

namespace Constants
{
    public class Enumerations
    {
        public enum ScenarioState { Null, Initialized, InProgress, Finished };

        public enum ArenaSide { Defending, Opposing };

        public enum ControllerType { Human, CPU };

        public enum Team { Player, Enemy, Neutral };

        public enum AssetID
        {
            CharacterCanvas,

            AK,
            Carbine,
            Revolver,
            Deagle,
            Glock,
            Shotgun,
            MP5,
            Uzi,

            Male_1,
            Male_2,
            Male_3,
            Male_4,
            Female_1,
            Female_2,
            Female_3,
            Female_4,

            Group_1,
            Group_2,
            Group_3,
            Group_4,
            Group_5,
        }


        public enum UIID
        {
            CharacterCanvas,
        }

        public enum WeaponID
        {
            Glock,
            None,

            //AK,
            //Carbine,
            //Revolver,
            //Deagle,
            //Shotgun,
            //MP5,
            //Uzi,
        }

        public enum CharacterModelID
        {
            Model_Male1,
            //Male_2,
            //Male_3,
            //Male_4,
            //Female_1,
            //Female_2,
            //Female_3,
            //Female_4,
        }

        public enum MarkerGroupID
        {
            Group_1,
            Group_2,
            Group_3,
            Group_4,
            Group_5,
        }

        public enum CharacterID
        {
            Player,
            Goon_Ally,
            Goon_Enemy,
            None
        }

        public enum EventID
        {
            GameModeChanged,
            GameStateChanged,
            EnvironmentChanged,
            GameSaved,
            None
        };

        public enum Environment
        {
            Safehouse,
            None
        }

        public enum GameMode
        {
            Root,
            GamePlay,
            Paused,
            Loading,
            None
        };

        public enum SceneType
        {
            Root,
            Environment,
            UI,
            None
        }

        public enum CharacterClothingType
        {
            Top,
            Bottom,
            Shoes,
            Hat,
            Accessories,
            None
        }
   
        public static string GetSceneNameFromEnvironmentID(Enumerations.Environment environment)
        {
            switch (environment)
            {
                case Environment.Safehouse:
                    return SceneName.Environment_Safehouse;
                default:
                    return null;
            }
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

        //ui
        public const string UI_GamePlay = "Scene_UI_GamePlay";
        public const string UI_Loading = "Scene_UI_Loading";
        public const string UI_Pause = "Scene_UI_Pause";
        //environments
        public const string Environment_Safehouse = "Scene_Safehouse";
        //debug
        public const string UI_DebugMenu = "Scene_UI_DebugMenu";

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
    }

    public class SaveKeys
    {
        public const string Player_Name = "Player_Name";
        public const string Player_Money = "Player_Money";
        public const string Player_Drugs = "Player_Drugs";
        public const string Player_Environment = "Player_Environment";

        public const string Game_Day = "Game_Day";
    }

    public class NPC
    {
        public enum TaskID { GoToRandomLocation, PerformIdle };
        public enum TaskState { ToDo, InProgress, Complete };
    }
}