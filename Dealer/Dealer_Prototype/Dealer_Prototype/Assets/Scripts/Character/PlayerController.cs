using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using TMPro;

public class PlayerController : CharacterComponent
{
    private GameObject SelectionPrefab;
    private Dictionary<Vector3, GameObject> Spawned;

    private static PlayerController _instance;

    public static PlayerController Instance { get { return _instance; } }

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

        SelectionPrefab = Resources.Load<GameObject>("NavPoint_Player");
        Spawned = new Dictionary<Vector3, GameObject>();

    }

    public override void OnNewDestination(Vector3 destination)
    {
        base.OnNewDestination(destination);

        SpawnSelectionPrefab(destination);
    }

    public override void OnReachedLocation(Vector3 location)
    {
        base.OnReachedLocation(location);

        if(Spawned.ContainsKey(location))
        {
            GameObject SelectionPoint = Spawned[location];
            Spawned.Remove(location);
            GameObject.Destroy(SelectionPoint);
        }
        
    }

    private void SpawnSelectionPrefab(Vector3 location)
    {
        GameObject SelectionEffect = Instantiate<GameObject>(SelectionPrefab, location, SelectionPrefab.transform.rotation, null);
        Spawned.Add(location, SelectionEffect);
    }
}
