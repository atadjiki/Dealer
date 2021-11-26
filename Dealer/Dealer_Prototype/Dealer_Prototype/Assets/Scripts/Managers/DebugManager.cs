using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using TMPro;
using UnityEngine;

public class DebugManager : MonoBehaviour
{
    private static DebugManager _instance;

    public static DebugManager Instance { get { return _instance; } }

    public bool LogCharacter = false;
    public bool LogInput = false;
    public bool LogAStar = false;
    public bool LogNavigator = false;
    public bool LogNPCManager = false;
    public bool LogDatabase = false;
    public bool LogSpawner = false;
    public bool LogCameraManager = false;

    public AstarPath _astarPath;

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
        if(_astarPath != null && LogAStar)
        {
            _astarPath.logPathResults = PathLog.Normal;
        }
    }
}
