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
        private static string Prefix_UI = "Prefabs/UI/";
        private static string Prefix_UI_Character = "Prefabs/UI/Character/";
        private static string Prefix_UI_Dialogue = "Prefabs/UI/Dialogue/";
        private static string Prefix_UI_Safehouse = "Prefabs/UI/Safehouse/";

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

        public static GameObject GetCharacterNavDecal()
        {
            return Resources.Load<GameObject>(Prefix_Gameplay + "CharacterNavDecal");
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

        public static GameObject GetDialogueCanvas()
        {
            return Resources.Load<GameObject>(Prefix_UI_Dialogue + "DialogueCanvas");
        }

        public static GameObject GetSafehouseCanvas()
        {
            return Resources.Load<GameObject>(Prefix_UI_Safehouse + "SafehouseCanvas");
        }

        public static GameObject GetTransitionCanvas()
        {
            return Resources.Load<GameObject>(Prefix_UI + "TransitionCanvas");
        }
    }
}
