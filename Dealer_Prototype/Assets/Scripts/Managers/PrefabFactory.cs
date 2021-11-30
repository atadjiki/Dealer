using Constants;
using UnityEngine;

public class PrefabFactory : MonoBehaviour
{
    private static PrefabFactory _instance;

    public static PrefabFactory Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }

        Build();
    }

    private void Build()
    {

    }

    private GameObject GetPrefabFromRegistryID(RegistryID ID)
    {
        string FilePath;
        if(DatabaseManager.Instance.GetResourcePathFromRegistryID(ID, out FilePath))
        {
            GameObject PrefabObject = Resources.Load<GameObject>(FilePath);
            return PrefabObject;
        }

        return null;
    }

    public GameObject CreatePrefab(RegistryID ID, Transform transform)
    {
        return Instantiate<GameObject>(GetPrefabFromRegistryID(ID), transform, false);
    }

    public GameObject CreatePrefab(RegistryID ID, Vector3 location, Quaternion rotation, Transform parent)
    {

        return Instantiate<GameObject>(GetPrefabFromRegistryID(ID), location, rotation, parent);
    }

    public GameObject GetCharacterPrefab(string ID, Transform transform)
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
