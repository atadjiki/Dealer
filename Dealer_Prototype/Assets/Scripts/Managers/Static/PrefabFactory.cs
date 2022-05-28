using System;
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
        string temp = "Model_" + ID;
        RegistryID registryID;

        if(Enum.TryParse<RegistryID>(temp, out registryID))
        {
            return CreatePrefab(registryID, transform);     
        }

        return null;
    }
}
