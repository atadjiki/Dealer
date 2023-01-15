using System.Collections;
using UnityEngine;

public class Cutscene : MonoBehaviour
{
    [SerializeField] private CutsceneNode _root;

    private CutsceneNode _currentNode;

    private System.Action OnCutsceneFinished;

    public void Begin(System.Action _OnCutsceneFinished)
    {
        OnCutsceneFinished = _OnCutsceneFinished;

        _currentNode = _root;

        ProcessCurrentNode();
    }

    private void ProcessCurrentNode()
    {
        StartCoroutine(Coroutine_ProcessCurrentNode());
    }

    private IEnumerator Coroutine_ProcessCurrentNode()
    {
        if (_currentNode != null)
        {
            foreach(CutsceneEvent cutsceneEvent in _currentNode.GetPreEvents())
            {
                yield return CutsceneHelper.ProcessCutsceneEvent(cutsceneEvent);
            }

            _currentNode.Setup(OnNodeComplete);
        }
        else
        {
            OnCutsceneFinished.Invoke();
        }

        yield return null;
    }

    private void OnNodeComplete()
    {
        StartCoroutine(Coroutine_OnNodeComplete());
    }

    private IEnumerator Coroutine_OnNodeComplete()
    {
        if(_currentNode != null)
        {
            foreach (CutsceneEvent cutsceneEvent in _currentNode.GetPostEvents())
            {
                yield return CutsceneHelper.ProcessCutsceneEvent(cutsceneEvent);
            }
        }

        _currentNode = _currentNode.GetNext();

        ProcessCurrentNode();

        yield return null;
    }
}