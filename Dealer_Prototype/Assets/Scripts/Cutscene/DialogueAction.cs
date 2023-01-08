using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Constants;

[System.Serializable]
public class DialogueData
{
    public Enumerations.CharacterID CharacterID;
    public string Line;
}

public class DialogueAction : CutsceneAction
{

    [SerializeField] private DialogueData _data;

    protected override IEnumerator Coroutine_PerformAction()
    {
        GameObject canvasObject = Instantiate<GameObject>(PrefabLibrary.GetDialogueCanvas());
        DialogueCanvas dialogueCanvas = canvasObject.GetComponent<DialogueCanvas>();
        dialogueCanvas.Setup(_data.CharacterID.ToString(), _data.Line, OnDialogAdvanced);

        return base.Coroutine_PerformAction();
    }

    private void OnDialogAdvanced()
    {
        Debug.Log("dialogue advanced");
        CompleteAction();
    }
}
