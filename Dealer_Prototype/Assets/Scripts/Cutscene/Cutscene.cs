using System.Collections;
using System.Collections.Generic;
using GameDelegates;
using Constants;
using UnityEngine;

public class Cutscene : MonoBehaviour
{
    [SerializeField] private CutsceneNode _root;

    private CutsceneNode _currentNode;

    private void Awake()
    {
        Global.OnGameEvent += OnGameEvent;

        _currentNode = _root;

        ProcessCurrentNode();
    }

    private void OnDestroy()
    {
        Global.OnGameEvent -= OnGameEvent;
    }

    private void ProcessCurrentNode()
    {
        if (_currentNode != null)
        {
            _currentNode.Setup(OnNodeComplete);
        }
        else
        {
            Debug.Log("cutscene finished");
        }
    }

    private void OnNodeComplete()
    {
        Debug.Log("next node");
        _currentNode = _currentNode.GetNext();

        ProcessCurrentNode();
    }

    private void OnGameEvent(Enumerations.GameEvent gameEvent)
    {
        if(gameEvent != Enumerations.GameEvent.None)
        {
            Debug.Log("game event! " + gameEvent.ToString());
        }
    }
}