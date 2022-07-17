using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Component
{
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
        Debug.Log(this.name + " loading...");
    }

    protected virtual void PerformSave()
    {
        Debug.Log(this.name + " saving...");
    }
}
