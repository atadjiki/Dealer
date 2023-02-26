using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitNode : CutsceneNode
{
    [Space]
    [SerializeField] public float waitTime;

    [Space]
    [SerializeField] private CutsceneNode _next;

    public override void Setup(Cutscene cutscene, Action OnComplete)
    {
        base.Setup(cutscene, OnComplete);

        StartCoroutine(PerformWait());
    }

    private IEnumerator PerformWait()
    {
        yield return new WaitForSeconds(waitTime);

        CompleteNode();
    }

    public override CutsceneNode GetNext()
    {
        return _next;
    }
}
