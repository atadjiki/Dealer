using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneNode : MonoBehaviour
{
    //a collection of cutscene actions
    [SerializeField] private DialogueAction _dialogueAction;

    private bool _readyToAdvance = false;

    [SerializeField] private CutsceneNode _next;

    private void Awake()
    {
        _dialogueAction.OnActionComplete += OnDialogueComplete;
    }

    public void ProcessActions()
    {
        _dialogueAction.PerformAction();
    }

    public void OnDialogueComplete()
    {
        Debug.Log("ready to advance");
        _readyToAdvance = true;
    }

    public bool ReadyToAdvance()
    {
        return _readyToAdvance;
    }

    public CutsceneNode GetNext()
    {
        return _next;
    }
}
