using System.Collections;
using UnityEngine;

public class Cutscene : MonoBehaviour
{
    [SerializeField] private CutsceneNode _root;

    private CutsceneNode _currentNode;

    private System.Action OnCutsceneFinished;

    private const float _fadeTime = 2.5f;

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
                UIUtility.RequestFadeFromBlack(_fadeTime);
                yield return new WaitForSeconds(_fadeTime);
            }

            foreach (CutsceneEvent cutsceneEvent in _currentNode.GetPreEvents())
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
                UIUtility.RequestFadeToBlack(_fadeTime);
                yield return new WaitForSeconds(_fadeTime);
            }

            foreach (CutsceneEvent cutsceneEvent in _currentNode.GetPostEvents())
            {
                CutsceneHelper.ProcessCutsceneEvent(cutsceneEvent);
            }
        }

        _currentNode = _currentNode.GetNext();

        ProcessCurrentNode();
    }
}
