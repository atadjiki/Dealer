using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneNode : MonoBehaviour
{
    //a collection of cutscene actions
    [SerializeField] protected List<CutsceneAction> _actions;

    private int _completed;

    private bool _readyToAdvance = false;

    [SerializeField] protected CutsceneNode _next;

    private void Awake()
    {
        foreach(CutsceneAction action in _actions)
        {
            action.OnActionComplete += OnActionComplete;
        }
    }

    public void ProcessActions()
    {
        foreach (CutsceneAction action in _actions)
        {
            action.PerformAction();
        }
    }

    public virtual void OnActionComplete()
    {
        _completed += 1;

        if(_completed >= _actions.Count)
        {
            _readyToAdvance = true;
        }
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
