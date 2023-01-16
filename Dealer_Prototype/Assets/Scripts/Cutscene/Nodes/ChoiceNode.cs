using Constants;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ChoicePair
{
    public string Text;
    public CutsceneNode Next;
}

[System.Serializable]
public class ChoiceActionData : CutsceneNodeData
{
    public List<ChoicePair> Choices = new List<ChoicePair>();
}

public class ChoiceNode : CutsceneNode
{
    [Space]
    [SerializeField] private ChoiceActionData Data;

    private int _selectedIndex = -1;

    public override void Setup(Cutscene cutscene, System.Action CompleteAction)
    {
        base.Setup(cutscene, CompleteAction);

        GameObject canvasObject = Instantiate<GameObject>(PrefabLibrary.GetChoiceCanvas());
        ChoiceCanvas choiceCanvas = canvasObject.GetComponent<ChoiceCanvas>();
        choiceCanvas.Setup(Data);
        choiceCanvas.onChoiceSelected += OnChoiceSelected;
    }

    protected override void CompleteNode()
    {
        base.CompleteNode();
    }

    public void OnChoiceSelected(int index)
    {
        _selectedIndex = index;
        CompleteNode();
    }

    public override CutsceneNode GetNext()
    {
        if(Data.Choices.Count > _selectedIndex)
        {
            return Data.Choices[_selectedIndex].Next;
        }

        return null;
    }
}
