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

    public void ProcessCurrentNode()
    {
        StartCoroutine(Coroutine_ProcessCurrentNode());
    }

    private IEnumerator Coroutine_ProcessCurrentNode()
    {
        if (_currentNode != null)
        {
            if(_currentNode.DoFadeIn())
            {
                UIUtility.RequestFadeFromBlack(_currentNode.GetFadeIn());
                yield return new WaitForSeconds(_currentNode.GetFadeIn());
            }

            foreach (CutsceneEvent cutsceneEvent in _currentNode.GetPreEvents().ToList())
            {
                CutsceneHelper.ProcessCutsceneEvent(cutsceneEvent);
            }

            _currentNode.Setup(OnNodeComplete);
        }
        else
        {
            OnCutsceneFinished.Invoke();
            Destroy(this.gameObject);
        }
    }

    private void OnNodeComplete()
    {
        StartCoroutine(Coroutine_OnNodeComplete());
    }

    private IEnumerator Coroutine_OnNodeComplete()
    {
        if (_currentNode != null)
        {
            if (_currentNode.DoFadeOut())
            {
                UIUtility.RequestFadeToBlack(_currentNode.GetFadeOut());
                yield return new WaitForSeconds(_currentNode.GetFadeOut());
            }

            foreach (CutsceneEvent cutsceneEvent in _currentNode.GetPostEvents().ToList())
            {
                CutsceneHelper.ProcessCutsceneEvent(cutsceneEvent);
            }
        }

        _currentNode = _currentNode.GetNext();

        ProcessCurrentNode();
    }
}
