namespace Constants
{
    public class PrefabPaths
    {
        public static string Path_Character_Decal = "Prefabs/Decals/Decal_Character";
        public static string Path_Character_Combat_Canvas = "Prefabs/UI/CharacterCombatCanvas";

        public static string Path_Weapon_Revolver = "Prefabs/Weapons/Model_Weapon_Revolver";
        public static string Path_Weapon_Pistol = "Prefabs/Weapons/Model_Weapon_Pistol";

        public static string Path_Model_Character_Mafia_Italian = "Prefabs/Character/Model_Character_Mafia_Italian";
        public static string Path_Model_Character_Mafia_British = "Prefabs/Character/Model_Character_Mafia_British";
        public static string Path_Model_Character_DEA_Male = "Prefabs/Character/Model_Character_DEA_Male";
        public static string Path_Model_Character_DEA_Female = "Prefabs/Character/Model_Character_DEA_Female";

        public static string GetWeaponByID(CharacterConstants.WeaponID type)
        {
            switch (type)
            {
                case CharacterConstants.WeaponID.Revolver:
                    return Path_Weapon_Revolver;
                case CharacterConstants.WeaponID.Pistol:
                    return Path_Weapon_Pistol;
                default:
                    return null;
            }
        }

        public static string GetCharacterModel(CharacterConstants.ModelID ID)
        {
            switch (ID)
            {
                case CharacterConstants.ModelID.MAFIA_ITALIAN:
                    return Path_Model_Character_Mafia_Italian;
                case CharacterConstants.ModelID.MAFIA_BRITISH:
                    return Path_Model_Character_Mafia_British;
                case CharacterConstants.ModelID.DEA_FEMALE:
                    return Path_Model_Character_DEA_Female;
                case CharacterConstants.ModelID.DEA_MALE:
                    return Path_Model_Character_DEA_Male;
                default:
                    return null;

            }
        }
    }
}