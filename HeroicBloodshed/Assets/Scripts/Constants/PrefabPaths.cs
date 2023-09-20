using static Constants;

public class PrefabPaths
{
    public static string Prefix_Prefabs = "Prefabs/";

    public static string Path_Character_Decal = "Prefabs/Decals/Decal_Character";

    public static string Path_Character_Encounter_Overhead = "Prefabs/Encounter/UI/Encounter_CurrentCharacter_Canvas";

    public static string Path_Weapon_Revolver = "Prefabs/Weapons/Model_Weapon_Revolver";
    public static string Path_Weapon_Pistol = "Prefabs/Weapons/Model_Weapon_Pistol";
    public static string Path_Weapon_SMG = "Prefabs/Weapons/Model_Weapon_SMG";

    public static string Path_Model_Character_Mafia_Italian = "Prefabs/Character/Model_Character_Mafia_Italian";
    public static string Path_Model_Character_Mafia_British = "Prefabs/Character/Model_Character_Mafia_British";
    public static string Path_Model_Character_DEA_Male = "Prefabs/Character/Model_Character_DEA_Male";
    public static string Path_Model_Character_DEA_Female = "Prefabs/Character/Model_Character_DEA_Female";

    public static string GetWeaponByID(WeaponID type)
    {
        switch (type)
        {
            case WeaponID.Revolver:
                return Path_Weapon_Revolver;
            case WeaponID.Pistol:
                return Path_Weapon_Pistol;
            case WeaponID.SMG:
                return Path_Weapon_SMG;
            default:
                return null;
        }
    }

    public static string GetCharacterModel(ModelID ID)
    {
        switch (ID)
        {
            case ModelID.MAFIA_ITALIAN:
                return Path_Model_Character_Mafia_Italian;
            case ModelID.MAFIA_BRITISH:
                return Path_Model_Character_Mafia_British;
            case ModelID.DEA_FEMALE:
                return Path_Model_Character_DEA_Female;
            case ModelID.DEA_MALE:
                return Path_Model_Character_DEA_Male;
            default:
                return null;

        }
    }
}