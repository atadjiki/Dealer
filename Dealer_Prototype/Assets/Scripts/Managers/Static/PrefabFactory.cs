using Constants;
using UnityEngine;

public class PrefabFactory : MonoBehaviour
{
    private static GameObject GetPrefabFromRegistryID(RegistryID ID)
    {
        string FilePath;
        if(Registry.GetResourcePathFromRegistryID(ID, out FilePath))
        {
            GameObject PrefabObject = Resources.Load<GameObject>(FilePath);
            return PrefabObject;
        }

        return null;
    }

    public static GameObject CreatePrefab(RegistryID ID, Transform transform)
    {
        return Instantiate<GameObject>(GetPrefabFromRegistryID(ID), transform, false);
    }

    public static GameObject CreatePrefab(RegistryID ID, Vector3 location, Quaternion rotation, Transform parent)
    {

        return Instantiate<GameObject>(GetPrefabFromRegistryID(ID), location, rotation, parent);
    }

    public static GameObject GetCharacterPrefab(string ID, Transform transform)
    {
        if (ID == CharacterConstants.CharacterID.Male_1.ToString())
        {
            return CreatePrefab(RegistryID.Model_Male_1, transform);
        }
        else if (ID == CharacterConstants.CharacterID.Female_1.ToString())
        {
            return CreatePrefab(RegistryID.Model_Female_1, transform);
        }

        return null;
    }
}
