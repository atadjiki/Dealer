using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutscenePlayer : MonoBehaviour
{
    [SerializeField] private List<Cutscene> _cutscenes;

    private System.Action OnAllScenesFinished;

    private int _currentIndex = 0;

    public void Setup(System.Action _OnAllScenesFinished)
    {
        OnAllScenesFinished = _OnAllScenesFinished;
    }

    public void ProcessNext()
    {
        if (_currentIndex < _cutscenes.Count)
        {
            Cutscene cutscene = _cutscenes[_currentIndex];
            cutscene.Begin(OnCutsceneFinished);
        }
        else
        {
            Debug.Log("finished with all cutscenes for today");
            if(OnAllScenesFinished != null) OnAllScenesFinished.Invoke();
        }
    }

    private void OnCutsceneFinished()
    {
        _currentIndex++;
        ProcessNext();
    }
}

