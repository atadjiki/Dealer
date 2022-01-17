namespace Constants
{
    public enum RegistryID
    {
        Player,
        Navigator,
        NavPoint,
        Interaction,
        NPC,
        Model_Male_1,
        Model_Female_1,
        Managers,
        CharacterCanvas,
        CM_Character,
        CharacterCameraRig,
        CharacterLight,
        SelectionComponent,
        BehaviorDecal,
        PerLevel_Managers,
        Static_Managers,
        StartMenu
    }

    public class Registry
    {
        public static bool GetResourcePathFromRegistryID(RegistryID RegistryID, out string PathString)
        {
           switch(RegistryID)
            {
                case RegistryID.CharacterCameraRig:
                    PathString = "Prefabs/Components/CharacterCameraRig";
                    return true;
                case RegistryID.CharacterLight:
                    PathString = "Prefabs/Components/CharacterLight";
                    return true;
                case RegistryID.CharacterCanvas:
                    PathString = "Prefabs/Components/CharacterCanvas";
                    return true;
                case RegistryID.CM_Character:
                    PathString = "Prefabs/Camera/CM_Character";
                    return true;
                case RegistryID.Interaction:
                    PathString = "Prefabs/Components/InteractionComponent";
                    return true;
                case RegistryID.Managers:
                    PathString = "Prefabs/LevelAssets/Managers";
                    return true;
                case RegistryID.Model_Female_1:
                    PathString = "Prefabs/Models/Model_Female1";
                    return true;
                case RegistryID.Model_Male_1:
                    PathString = "Prefabs/Models/Model_Male1";
                    return true;
                case RegistryID.Navigator:
                    PathString = "Prefabs/Components/Navigator";
                    return true;
                case RegistryID.NavPoint:
                    PathString = "Prefabs/Navigation/NavPoint";
                    return true;
                case RegistryID.BehaviorDecal:
                    PathString = "Prefabs/Behavior/BehaviorDecal";
                    return true;
                case RegistryID.NPC:
                    PathString = "Prefabs/Characters/NPC_Empty";
                    return true;
                case RegistryID.Player:
                    PathString = "Prefabs/Characters/Player_Empty";
                    return true;
                case RegistryID.SelectionComponent:
                    PathString = "Prefabs/Components/SelectionComponent";
                    return true;
                case RegistryID.PerLevel_Managers:
                    PathString = "Prefabs/LevelAssets/PerLevel_Managers";
                    return true;
                case RegistryID.Static_Managers:
                    PathString = "Prefabs/LevelAssets/Static_Managers";
                    return true;
                case RegistryID.StartMenu:
                    PathString = "Prefabs/LevelAssets/StartMenu";
                    return true;
                default:
                    PathString = "NOT FOUND";
                    return false;
            }
        }
    }
}
