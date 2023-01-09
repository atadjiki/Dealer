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

        //private static string Prefix_UI = "Prefabs/UI/";
        private static string Prefix_UI_CityMap = "Prefabs/UI/CityMap/";
        private static string Prefix_UI_Character = "Prefabs/UI/Character/";
        private static string Prefix_UI_Dialogue = "Prefabs/UI/Dialogue/";
        private static string Prefix_UI_GameMode = "Prefabs/UI/GameMode/";
        private static string Prefix_UI_Safehouse = "Prefabs/UI/Safehouse/";
        private static string Prefix_UI_Utility = "Prefabs/UI/Utility/";

        //character models

        private static string GetResourcePath(Enumerations.CharacterModelID ID)
        {
            switch(ID)
            {
                case Enumerations.CharacterModelID.Model_Male1:
                case Enumerations.CharacterModelID.Model_Male_Player:
                case Enumerations.CharacterModelID.Model_Male_Police:
                case Enumerations.CharacterModelID.Model_Male_Customer:
                    return Prefix_CharacterModels + ID.ToString();
            }

            Debug.LogError("Prefab Library - We should not be hitting this line!");
            return null;
        }

        public static GameObject GetCharacterModelByID(Enumerations.CharacterModelID ID)
        {
            string ModelPath = GetResourcePath(ID);
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

        public static GameObject GetMouseContextCanvas()
        {
            return Resources.Load<GameObject>(Prefix_UI_Character + "MouseContextCanvas");
        }

        public static GameObject GetNavigatorComponent()
        {
            return Resources.Load<GameObject>(Prefix_Character + "NavigatorComponent");
        }

        public static GameObject GetDialogueCanvas()
        {
            return Resources.Load<GameObject>(Prefix_UI_Dialogue + "DialogueCanvas");
        }

        public static GameObject GetChoiceCanvas()
        {
            return Resources.Load<GameObject>(Prefix_UI_Dialogue + "ChoiceCanvas");
        }

        public static GameObject GetStationCanvas()
        {
            return Resources.Load<GameObject>(Prefix_UI_Safehouse + "StationCanvas");
        }

        public static GameObject GetTransitionCanvas()
        {
            return Resources.Load<GameObject>(Prefix_UI_Utility + "TransitionCanvas");
        }

        public static GameObject GetOKCancelCanvas()
        {
            return Resources.Load<GameObject>(Prefix_UI_Utility + "OkCancelDialogCanvas");
        }
        
        public static GameObject GetCityMapCanvas()
        {
            return Resources.Load<GameObject>(Prefix_UI_CityMap + "CityMapCanvas");
        }

        public static GameObject GetLoadingScreen()
        {
            return Resources.Load<GameObject>(Prefix_UI_GameMode + "UI_Panel_Loading");
        }
    }
}
