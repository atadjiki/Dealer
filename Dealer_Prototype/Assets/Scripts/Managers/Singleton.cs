using System.Collections;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Component
{

    public bool debug = false;
    
    private static T _instance;
    public static T Instance
    {
        get
        {
            return _instance;
        }
    }

    protected virtual void Awake()
    {
        _instance = this as T;
    }

    protected virtual void Start()
    {
        PerformLoad();
    }

    protected virtual void OnApplicationQuit()
    {
        PerformSave();
    }

    protected virtual void PerformLoad()
    {
        if(debug) Debug.Log(this.name + " loading...");
    }

    protected virtual void PerformSave()
    {
        if (debug) Debug.Log(this.name + " saving...");
    }
}