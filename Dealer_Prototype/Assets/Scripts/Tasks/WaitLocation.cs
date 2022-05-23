using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitLocation : MarkedLocation
{
    private void Awake()
    {
        PartyManager.Instance.RegisterLocation(this);
    }

    private void OnDestroy()
    {
        PartyManager.Instance.UnRegisterLocation(this);
    }
}
