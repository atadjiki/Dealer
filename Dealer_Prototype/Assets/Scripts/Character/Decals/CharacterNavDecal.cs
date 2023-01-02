using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine;

public class CharacterNavDecal : CharacterGroundDecal
{
    NavigatorComponent _navigator;

    public void Setup(NavigatorComponent navigator, Vector3 location)
    {
        _navigator = navigator;
        _navigator.OnDestinationReachedDelegate += PathComplete;

        this.transform.position = location;     
    }

    private void PathComplete()
    {
        _navigator.OnDestinationReachedDelegate -= PathComplete;
        Destroy(this.gameObject);
    }
}
