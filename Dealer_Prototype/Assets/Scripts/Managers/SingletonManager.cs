using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonManager : MonoBehaviour
{
    internal virtual void Build()
    {
        GameStateManager.Instance.onLevelStart += OnLevelStart;
    }

    internal virtual void OnLevelStart()
    {

    }
}
