using Constants;
using System;
using UnityEngine;

[System.Serializable]
public class DialogueData : CutsceneNodeData
{
    public Enumerations.CharacterID CharacterID;
}

public class DialogueNode : CutsceneNode
{
    [Space]
    [SerializeField] private DialogueData _data;

    [Space]
    [SerializeField] private CutsceneNode _next;

    public override void Setup(Cutscene cutscene, Action OnComplete)
    {
       base.Setup(cutscene, OnComplete);

       GameObject canvasObject = Instantiate<GameObject>(PrefabLibrary.GetDialogueCanvas());
       DialogueCanvas dialogueCanvas = canvasObject.GetComponent<DialogueCanvas>();
       dialogueCanvas.Setup(_data.CharacterID, _data.MainText, CompleteNode);
    }

    public override CutsceneNode GetNext()
    {
        return _next;
    }
}
