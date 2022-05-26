using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitLocation : MarkedLocation
{
    public float minWaitTime;
    public float maxWaitTime;

    private void Awake()
    {
        if(LevelManager.IsManagerLoaded())
        {
            CharacterManager.Instance.RegisterLocation(this);
        }
        else
        {
            this.enabled = false;
        }
    }

    private void OnDestroy()
    {
        if (LevelManager.IsManagerLoaded())
        {
            CharacterManager.Instance.UnRegisterLocation(this);
        }
    }

    public float GetWaitTime()
    {
        return Random.Range(minWaitTime, maxWaitTime);
    }
}
