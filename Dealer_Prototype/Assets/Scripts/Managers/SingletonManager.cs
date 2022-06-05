using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonManager : MonoBehaviour
{ 
    private static SingletonManager _instance;
    public static SingletonManager Instance { get { return _instance; } }

    private List<Manager> managers;
    private int buildCount = 0;
    private bool allowUpdate = false;

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

    public void Build()
    {
        //get all the managers
        managers = new List<Manager>(GetComponentsInChildren<Manager>());

        //make sure none of them are updating
        ActivateManagers(false);

        //assign delegates so we can get the callback
        AssignManagerDelegates();

        //at this stage, the managers are initializing variables, etc
        BuildManagers();
    }

    public void OnManagerBuildComplete(Manager manager)
    {
        buildCount += 1;

        if (AreManagersBuilt())
        {
//            Debug.Log(this.name + " | Managers Built = " + buildCount + "/" + managers.Count);

            int delegateCount = GatherManagerDelegates();

    //        Debug.Log(this.name + " | Assigned " + delegateCount + " delegates");

            //at this stage, the managers are all registered and ready to go, so we can activate them
            ActivateManagers(true);

            allowUpdate = true;
        }
    }

    private void AssignManagerDelegates()
    {
        foreach (Manager manager in managers)
        {
            manager.onBuildComplete += OnManagerBuildComplete;
        }
    }

    private int GatherManagerDelegates()
    {
        int delegateCount = 0;

        foreach (Manager manager in managers)
        {
            delegateCount += manager.AssignDelegates();
        }

        return delegateCount;
    }

    private void BuildManagers()
    {
        foreach (Manager manager in managers)
        {
            manager.Build();
        }
    }

    public void ActivateManagers(bool flag)
    {
        foreach (Manager manager in managers)
        {
            if (flag)
            {
                manager.Activate();
            }
            else
            {
                manager.DeActivate();
            }
        }
    }

    public bool AreManagersBuilt()
    {
        if (managers.Count == 0) { return false; }
        if (buildCount == 0) { return false; }

        return buildCount == managers.Count;
    }

    private void Update()
    {
        if(allowUpdate)
        {
            foreach(Manager manager in managers)
            {
                manager.PerformUpdate(Time.unscaledDeltaTime);
            }
        }
    }
}
