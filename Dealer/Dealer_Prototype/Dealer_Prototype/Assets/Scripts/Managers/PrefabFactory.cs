using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine;

public class PrefabFactory : MonoBehaviour
{
    private static PrefabFactory _instance;

    public static PrefabFactory Instance { get { return _instance; } }

    //Prefabs
    private GameObject Prefab_CharacterCamera;
    private GameObject Prefab_NavPoint;

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
        Prefab_CharacterCamera = Resources.Load<GameObject>(ResourcePaths.CM_Character);
        Prefab_NavPoint = Resources.Load<GameObject>(ResourcePaths.NavPoint);

    }

    private GameObject GetPrefabFromEnum(Prefab prefab)
    {
        if (prefab == Prefab.CM_Character)
        {
            return Prefab_CharacterCamera;
        }
        else if (prefab == Prefab.NavPoint)
        {
            return Prefab_NavPoint;
        }

        return null;
    }

    public GameObject CreatePrefab(Prefab prefab, Transform transform)
    {
        return Instantiate<GameObject>(GetPrefabFromEnum(prefab), transform);
    }

    public GameObject CreatePrefab(Prefab prefab, Vector3 location, Quaternion rotation)
    {

        return Instantiate<GameObject>(GetPrefabFromEnum(prefab), location, rotation, null);
    }
}
