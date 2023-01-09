using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Constants;

[System.Serializable]
public class ChoiceData
{
    public CutsceneNode Next;
    public string Text;
}

[System.Serializable]
public class ChoiceActionData
{
    public string Prompt;

    public ChoiceData A;
    public ChoiceData B;
    public ChoiceData C;
    public ChoiceData D;
}

public class ChoiceAction : CutsceneAction
{
    [SerializeField] private ChoiceActionData Data;

    private CutsceneNode next;

    protected override IEnumerator Coroutine_PerformAction()
    {
        GameObject canvasObject = Instantiate<GameObject>(PrefabLibrary.GetChoiceCanvas());
        ChoiceCanvas choiceCanvas = canvasObject.GetComponent<ChoiceCanvas>();
        choiceCanvas.Setup(Data.Prompt, Data.A.Text, Data.B.Text, Data.C.Text, Data.D.Text);
        choiceCanvas.onChoiceSelected += OnChoiceSelected;

        return base.Coroutine_PerformAction();
    }

    public void OnChoiceSelected(int index)
    {

    }
}
