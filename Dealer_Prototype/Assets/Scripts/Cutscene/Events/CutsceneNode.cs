using System.Collections;
using System.Collections.Generic;
using Constants;
using GameDelegates;
using UnityEngine;

[System.Serializable]
public class CutsceneNodeData
{
    [TextArea(5, 10)]
    public string MainText;
}

public class CutsceneNode : MonoBehaviour
{
    protected System.Action _OnCompleteAction;

    [SerializeField] protected List<CutsceneEvent> _Events;

    public virtual void Setup(System.Action OnComplete) 
    {
        _OnCompleteAction = OnComplete;
    }

    protected virtual void CompleteNode()
    {
        foreach(CutsceneEvent cutsceneEvent in _Events)
        {
            CutsceneHelper.ProcessCutsceneEvent(cutsceneEvent);
        }

        _OnCompleteAction.Invoke();
    }

    public virtual CutsceneNode GetNext(){ return null; }
}
