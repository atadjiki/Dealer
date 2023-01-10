using Constants;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ActionNodeData
{
    [TextArea(5, 10)]
    public string Text;
}

public class ActionNode : CutsceneNode
{
    [SerializeField] private ActionNodeData _data;

    [SerializeField] private CutsceneNode _next;

    public override void Setup(Action OnComplete)
    {
        base.Setup(OnComplete);

        GameObject canvasObject = Instantiate<GameObject>(PrefabLibrary.GetActionCanvas());
        ActionCanvas actionCanvas = canvasObject.GetComponent<ActionCanvas>();
        actionCanvas.Setup(_data.Text, CompleteNode);
    }

    public override CutsceneNode GetNext()
    {
        return _next;
    }
}
