using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using TMPro;

public class PlayerController : CharacterComponent
{
    private GameObject SelectionPrefab;

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

        SelectionPrefab = Resources.Load<GameObject>("SelectionPrefab");

        DontDestroyOnLoad(this.gameObject);
    }
}
