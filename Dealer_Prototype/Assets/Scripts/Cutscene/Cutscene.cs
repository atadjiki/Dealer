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

        StartCoroutine(ProcessCurrentNode());
    }

    public IEnumerator ProcessCurrentNode()
    {
        while (_currentNode != null)
        {
            _currentNode.ProcessActions();

            yield return new WaitUntil(() => _currentNode.ReadyToAdvance());

            Debug.Log("next node");

            _currentNode = _currentNode.GetNext();
        }

        Debug.Log("cutscene finished");
    }
}