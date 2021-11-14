namespace Constants
{
    public enum Prefab { CM_Character, NavPoint, NPC, Model_Male, Model_Female, Character_Canvas };

    public class ResourcePaths
    {
        //Camera
        public static string CM_Character = "Prefabs/Camera/CM_Character";

        //Characters
        public static string NPC = "Prefabs/Characters/NPC_Empty";
        public static string Player = "Prefabs/Characters/Player";

        //Components
        public static string Navigator = "Prefabs/Components/Navigator";
        public static string CharacterCanvas = "Prefabs/Components/CharacterCanvas";

        //Level Assets
        public static string Managers = "Prefabs/LevelAssets/Managers";

        //Models
        public static string Model_Male1 = "Prefabs/Models/Model_Male1";
        public static string Model_Female1 = "Prefabs/Models/Model_Female1";

        //Navigation
        public static string NavPoint = "Prefabs/Navigation/NavPoint";

       
    }
}
