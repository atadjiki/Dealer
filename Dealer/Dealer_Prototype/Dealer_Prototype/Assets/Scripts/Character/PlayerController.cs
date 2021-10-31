using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using TMPro;

public class PlayerController : CharacterComponent
{
    private GameObject NavPoint_Prefab;

    private static PlayerController _instance;

    public static PlayerController Instance { get { return _instance; } }

    private HashSet<GameObject> NavPointPrefabs;

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
        Initialize();

        NavPoint_Prefab = Resources.Load<GameObject>("NavPoint_Player");
        NavPointPrefabs = new HashSet<GameObject>();

    }

    public override void OnNewDestination(Vector3 destination)
    {
        base.OnNewDestination(destination);

        //get rid of any existing prefabs that are out there first
        foreach(GameObject todestroy in NavPointPrefabs)
        {
            Destroy(todestroy);
        }

        SpawnNavPointPrefab(destination);
    }

    public override void OnDestinationReached(Vector3 destination)
    {
        base.OnDestinationReached(destination);

        //get rid of any existing prefabs that are out there first
        foreach (GameObject todestroy in NavPointPrefabs)
        {
            Destroy(todestroy);
        }
    }

    private void SpawnNavPointPrefab(Vector3 prefabLocation)
    {
        GameObject NavPointEffect = Instantiate<GameObject>(NavPoint_Prefab, prefabLocation, NavPoint_Prefab.transform.rotation, null);
        NavPointPrefabs.Add(NavPointEffect);
    }
}
