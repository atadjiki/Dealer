using System.Collections;
using System.Collections.Generic;
using GameDelegates;
using UnityEngine;

public class CutsceneAction : MonoBehaviour
{
    public CutsceneActionComplete OnActionComplete;

    public void PerformAction()
    {
        StartCoroutine(Coroutine_PerformAction());
    }

    protected virtual IEnumerator Coroutine_PerformAction()
    {
        yield return null;
    }

    protected virtual void CompleteAction()
    {
        OnActionComplete.Invoke();
    }
}

