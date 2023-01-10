using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cutscene : MonoBehaviour
{
    [SerializeField] private CutsceneNode _root;

    private CutsceneNode _currentNode;

    private void Awake()
    {
        _currentNode = _root;

        ProcessCurrentNode();
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
}