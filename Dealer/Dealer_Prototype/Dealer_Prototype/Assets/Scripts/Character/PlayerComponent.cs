using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerComponent : CharacterComponent
{
    private static PlayerComponent _instance;

    public static PlayerComponent Instance { get { return _instance; } }

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
    }

    public override void OnNewDestination(Vector3 destination)
    {
        base.OnNewDestination(destination);
    }

    public override void OnDestinationReached(Vector3 destination)
    {
        base.OnDestinationReached(destination);
    }
}
