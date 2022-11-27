using Constants;
using UnityEngine;

namespace Constants
{
    public static class PrefabLibrary
    {
        private static string Prefix_Debug = "Debug/";
        private static string Prefix_CharacterModels = "Prefabs/CharacterModels/";

        //character models

        private static string GetResourcePath(Enumerations.CharacterModelID ID)
        {
            switch(ID)
            {
                case Enumerations.CharacterModelID.Model_Male1:
                    return Prefix_CharacterModels + ID.ToString();
            }

            Debug.LogError("Prefab Library - We should not be hitting this line!");
            return null;
        }

        public static GameObject GetCharacterModelByID(Enumerations.CharacterModelID ID)
        {
            string ModelPath = GetResourcePath(ID);
            Debug.Log(ModelPath);
            return Resources.Load<GameObject>(ModelPath);
        }

        //debug

        public static GameObject GetDirectionalMarker()
        {
            return Resources.Load<GameObject>(Prefix_Debug + "DirectionalMarker");
        }
    }
}
