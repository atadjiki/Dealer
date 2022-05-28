namespace Constants
{
    public enum RegistryID
    {
        Navigator,
        Interaction,
        NPC,
        Model_Male_1,
        Model_Male_2,
        Model_Male_3,
        Model_Male_4,
        Model_Female_1,
        Model_Female_2,
        Model_Female_3,
        Model_Female_4,
    }

    public class Registry
    {
        public static bool GetResourcePathFromRegistryID(RegistryID RegistryID, out string PathString)
        {
           switch(RegistryID)
            {
                case RegistryID.Interaction:
                    PathString = "Prefabs/Components/CharacterInteractionComponent";
                    return true;
                case RegistryID.Model_Female_1:
                    PathString = "Prefabs/Models/Model_Female1";
                    return true;
                case RegistryID.Model_Female_2:
                    PathString = "Prefabs/Models/Model_Female2";
                    return true;
                case RegistryID.Model_Female_3:
                    PathString = "Prefabs/Models/Model_Female3";
                    return true;
                case RegistryID.Model_Female_4:
                    PathString = "Prefabs/Models/Model_Female4";
                    return true;
                case RegistryID.Model_Male_1:
                    PathString = "Prefabs/Models/Model_Male1";
                    return true;
                case RegistryID.Model_Male_2:
                    PathString = "Prefabs/Models/Model_Male2";
                    return true;
                case RegistryID.Model_Male_3:
                    PathString = "Prefabs/Models/Model_Male3";
                    return true;
                case RegistryID.Model_Male_4:
                    PathString = "Prefabs/Models/Model_Male4";
                    return true;
                case RegistryID.Navigator:
                    PathString = "Prefabs/Components/Navigator";
                    return true;
                case RegistryID.NPC:
                    PathString = "Prefabs/Characters/Character_Empty";
                    return true;
                default:
                    PathString = "NOT FOUND";
                    return false;
            }
        }
    }
}
