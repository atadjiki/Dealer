using GameDelegates;
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
        if (_currentNode != null)
        {
            _currentNode.Setup(OnNodeComplete);
        }
        else
        {
            OnCutsceneFinished.Invoke();
        }
    }

    private void OnNodeComplete()
    {
        Debug.Log("next node");
        _currentNode = _currentNode.GetNext();

        ProcessCurrentNode();
    }
}