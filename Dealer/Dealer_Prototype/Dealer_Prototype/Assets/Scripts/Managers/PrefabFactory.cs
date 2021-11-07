using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine;

public class PrefabFactory : MonoBehaviour
{
    private static PrefabFactory _instance;

    public static PrefabFactory Instance { get { return _instance; } }

    //Prefabs
    private GameObject Prefab_Camera_Character;
    private GameObject Prefab_Navigation_NavPoint;
    private GameObject Prefab_Model_Male1;
    private GameObject Prefab_Character_NPC;

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
        Prefab_Camera_Character = Resources.Load<GameObject>(ResourcePaths.CM_Character);
        Prefab_Navigation_NavPoint = Resources.Load<GameObject>(ResourcePaths.NavPoint);
        Prefab_Model_Male1 = Resources.Load<GameObject>(ResourcePaths.Model_Male1);
        Prefab_Character_NPC = Resources.Load<GameObject>(ResourcePaths.NPC);
    }

    private GameObject GetPrefabFromEnum(Prefab prefab)
    {
        if (prefab == Prefab.CM_Character)
        {
            return Prefab_Camera_Character;
        }
        else if (prefab == Prefab.NavPoint)
        {
            return Prefab_Navigation_NavPoint;
        }
        else if(prefab == Prefab.Model_Male)
        {
            return Prefab_Model_Male1;
        }
        else if(prefab == Prefab.NPC)
        {
            return Prefab_Character_NPC;
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
