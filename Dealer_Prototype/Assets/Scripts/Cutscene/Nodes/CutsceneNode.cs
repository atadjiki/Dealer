using System.Collections.Generic;
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

    [SerializeField] protected List<CutsceneEvent> _PreEvents;

    [SerializeField] protected List<CutsceneEvent> _PostEvents;

    [SerializeField] protected bool _FadeIn;

    [SerializeField] protected bool FadeOut;

    public virtual void Setup(System.Action OnComplete) 
    {
        _OnCompleteAction = OnComplete;
    }

    protected virtual void CompleteNode()
    {
        _OnCompleteAction.Invoke();
    }

    public List<CutsceneEvent> GetPreEvents()
    {
        return _PreEvents;
    }

    public List<CutsceneEvent> GetPostEvents()
    {
        return _PostEvents;
    }

    public virtual CutsceneNode GetNext(){ return null; }
}
