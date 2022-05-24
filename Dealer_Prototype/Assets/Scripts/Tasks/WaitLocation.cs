using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitLocation : MarkedLocation
{
    private void Awake()
    {
        CharacterManager.Instance.RegisterLocation(this);
    }

    private void OnDestroy()
    {
        CharacterManager.Instance.UnRegisterLocation(this);
    }
}
