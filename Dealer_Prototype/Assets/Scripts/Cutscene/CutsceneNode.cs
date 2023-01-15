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

    [SerializeField] private Enumerations.GameEvent _OnCompleteEvent = Enumerations.GameEvent.None;

    public virtual void Setup(System.Action OnComplete) 
    {
        _OnCompleteAction = OnComplete;
    }

    protected virtual void CompleteNode()
    {
        if(Global.OnGameEvent != null) Global.OnGameEvent(_OnCompleteEvent);
        _OnCompleteAction.Invoke();
    }

    public virtual CutsceneNode GetNext(){ return null; }
}
