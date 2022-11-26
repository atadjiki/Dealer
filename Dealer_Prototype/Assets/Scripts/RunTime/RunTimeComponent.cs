using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunTimeComponent : MonoBehaviour
{
    private bool _initialized = false;

    public void Initialize()
    {
        StartCoroutine(PerformInitialize());
    }

    protected virtual IEnumerator PerformInitialize()
    {
        _initialized = true;

        yield return null;
    }

    public bool HasInitialized()
    {
        return _initialized;
    }
}
