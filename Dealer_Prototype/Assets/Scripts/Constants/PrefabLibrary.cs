using Constants;
using UnityEngine;

namespace Constants
{
    public static class PrefabLibrary
    {
        private static string Prefix_Debug = "Debug/";
        private static string Prefix_Character = "Prefabs/Character/";
        private static string Prefix_CharacterModels = "Prefabs/CharacterModels/";
        private static string Prefix_Gameplay = "Prefabs/Gameplay/";
        private static string Prefix_UI_Character = "Prefabs/UI/Character/";

        //character models

        private static string GetResourcePath(Enumerations.CharacterModelID ID)
        {
            switch(ID)
            {
                case Enumerations.CharacterModelID.Model_Male1:
                case Enumerations.CharacterModelID.Model_Male_Player:
                case Enumerations.CharacterModelID.Model_Male_Police:
                    return Prefix_CharacterModels + ID.ToString();
            }

            Debug.LogError("Prefab Library - We should not be hitting this line!");
            return null;
        }

        public static GameObject GetCharacterModelByID(Enumerations.CharacterModelID ID)
        {
            string ModelPath = GetResourcePath(ID);
        //    Debug.Log(ModelPath);
            return Resources.Load<GameObject>(ModelPath);
        }

        //debug

        public static GameObject GetDirectionalMarker()
        {
            return Resources.Load<GameObject>(Prefix_Debug + "DirectionalMarker");
        }

        public static GameObject GetCharacterGroundDecal()
        {
            return Resources.Load<GameObject>(Prefix_Gameplay + "CharacterGroundDecal");
        }

        public static GameObject GetCharacterCanvas()
        {
            return Resources.Load<GameObject>(Prefix_UI_Character + "CharacterCanvas");
        }

        public static GameObject GetPlayerCanvas()
        {
            return Resources.Load<GameObject>(Prefix_UI_Character + "PlayerCanvas");
        }

        public static GameObject GetNavigatorComponent()
        {
            return Resources.Load<GameObject>(Prefix_Character + "NavigatorComponent");
        }
    }
}
