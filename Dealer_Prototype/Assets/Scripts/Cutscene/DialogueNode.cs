using Constants;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueData
{
    public Enumerations.CharacterID CharacterID;

    [TextArea(5, 10)]
    public string Line;
}

public class DialogueNode : CutsceneNode
{
    [SerializeField] private DialogueData _data;

    [SerializeField] private CutsceneNode _next;

    public override void Setup(Action OnComplete)
    {
       base.Setup(OnComplete);

       GameObject canvasObject = Instantiate<GameObject>(PrefabLibrary.GetDialogueCanvas());
       DialogueCanvas dialogueCanvas = canvasObject.GetComponent<DialogueCanvas>();
       dialogueCanvas.Setup(_data.CharacterID, _data.Line, CompleteNode);
    }

    public override CutsceneNode GetNext()
    {
        return _next;
    }
}
