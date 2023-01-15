using Constants;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionNode : CutsceneNode
{
    [SerializeField] private CutsceneNodeData _data;

    [Space]
    [SerializeField] private CutsceneNode _next;

    public override void Setup(Action OnComplete)
    {
        base.Setup(OnComplete);

        GameObject canvasObject = Instantiate<GameObject>(PrefabLibrary.GetActionCanvas());
        ActionCanvas actionCanvas = canvasObject.GetComponent<ActionCanvas>();
        actionCanvas.Setup(_data.MainText, CompleteNode);
    }

    public override CutsceneNode GetNext()
    {
        return _next;
    }
}
