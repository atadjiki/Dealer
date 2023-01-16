using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CutsceneNodeData
{
    [TextArea(5, 10)]
    public string MainText;
}

[System.Serializable]
public class NodeEvents
{
    public AnimationEvent AnimEvent;
    public CameraEvent CamEvent;
    public TransactionEvent TransactionEvent;
    public CharacterVisiblityEvent VisiblityEvent;

    public List<CutsceneEvent> ToList()
    {
        return new List<CutsceneEvent>()
        {
            AnimEvent,
            CamEvent,
            TransactionEvent,
            VisiblityEvent,
        };
    }
}

public class CutsceneNode : MonoBehaviour
{
    protected System.Action _OnCompleteAction;

    [SerializeField] protected NodeEvents _PreEvents;

    [SerializeField] protected NodeEvents _PostEvents;

    [SerializeField] protected float _FadeIn;

    [SerializeField] protected float _FadeOut;

    public virtual void Setup(Cutscene cutscene, System.Action OnComplete){ _OnCompleteAction = OnComplete; }

    protected virtual void CompleteNode(){ _OnCompleteAction.Invoke(); }

    public NodeEvents GetPreEvents(){ return _PreEvents; }

    public NodeEvents GetPostEvents(){ return _PostEvents; }

    public virtual CutsceneNode GetNext(){ return null; }

    public bool DoFadeIn() { return _FadeIn > 0; }

    public bool DoFadeOut() { return _FadeOut > 0; }

    public float GetFadeIn() { return _FadeIn; }

    public float GetFadeOut() { return _FadeOut; }
}
