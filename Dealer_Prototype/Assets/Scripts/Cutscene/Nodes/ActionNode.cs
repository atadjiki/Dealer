using Constants;
using System;
using UnityEngine;

public class ActionNode : CutsceneNode
{
    [Space]
    [SerializeField] private CutsceneNodeData _data;

    [Space]
    [SerializeField] private CutsceneNode _next;

    public override void Setup(Cutscene cutscene, Action OnComplete)
    {
        base.Setup(cutscene, OnComplete);

        GameObject canvasObject = Instantiate<GameObject>(PrefabLibrary.GetActionCanvas());
        ActionCanvas actionCanvas = canvasObject.GetComponent<ActionCanvas>();
        actionCanvas.Setup(_data.MainText, CompleteNode);
    }

    public override CutsceneNode GetNext()
    {
        return _next;
    }
}
