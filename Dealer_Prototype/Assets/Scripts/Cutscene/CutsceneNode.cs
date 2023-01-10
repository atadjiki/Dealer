using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneNode : MonoBehaviour
{
    protected System.Action _OnCompleteAction;

    public virtual void Setup(System.Action OnComplete) 
    {
        _OnCompleteAction = OnComplete;
    }

    protected virtual void CompleteNode()
    {
        _OnCompleteAction.Invoke();
    }

    public virtual CutsceneNode GetNext(){ return null; }
}
