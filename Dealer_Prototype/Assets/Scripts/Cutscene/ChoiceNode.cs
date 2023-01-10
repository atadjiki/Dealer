using Constants;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ChoiceActionData
{
    public string Prompt;

    public string Text_A;
    public string Text_B;
    public string Text_C;
    public string Text_D;
}

public class ChoiceNode : CutsceneNode
{
    [SerializeField] private ChoiceActionData Data;

    [SerializeField] private CutsceneNode Choice_A;
    [SerializeField] private CutsceneNode Choice_B;
    [SerializeField] private CutsceneNode Choice_C;
    [SerializeField] private CutsceneNode Choice_D;

    private int _selectedIndex = -1;

    public override void Setup(System.Action CompleteAction)
    {
        base.Setup(CompleteAction);

        GameObject canvasObject = Instantiate<GameObject>(PrefabLibrary.GetChoiceCanvas());
        ChoiceCanvas choiceCanvas = canvasObject.GetComponent<ChoiceCanvas>();
        choiceCanvas.Setup(Data);
        choiceCanvas.onChoiceSelected += OnChoiceSelected;
    }

    public void OnChoiceSelected(int index)
    {
        Debug.Log("choice selected");
        _selectedIndex = index;
        CompleteNode();
    }

    public override CutsceneNode GetNext()
    {
        switch(_selectedIndex)
        {
            case 0:
                return Choice_A;
            case 1:
                return Choice_B;
            case 2:
                return Choice_C;
            case 3:
                return Choice_D;
            default:
                return null;
        }
    }
}
