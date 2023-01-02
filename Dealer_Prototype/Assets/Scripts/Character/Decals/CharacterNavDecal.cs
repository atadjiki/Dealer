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
        _navigator.OnNewDestinationDelegate += NewDestination;

        this.transform.position = location;     
    }

    private void NewDestination(Vector3 destination)
    {
        PathComplete();
    }

    private void PathComplete()
    {
        if(this.gameObject != null)
        {
            _navigator.OnNewDestinationDelegate -= NewDestination;
            _navigator.OnDestinationReachedDelegate -= PathComplete;
            Destroy(this.gameObject);

        }

    }
}
