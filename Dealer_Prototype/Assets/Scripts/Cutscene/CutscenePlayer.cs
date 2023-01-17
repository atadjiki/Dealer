using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutscenePlayer : MonoBehaviour
{
    [SerializeField] private List<Cutscene> _cutscenes;

    [SerializeField] private bool PlayOnAwake;

    private System.Action OnAllScenesFinished;

    private int _currentIndex = 0;

    private void Awake()
    {
        if(PlayOnAwake)
        {
            ProcessNext();
        }
    }

    public void Setup(System.Action _OnAllScenesFinished)
    {
        OnAllScenesFinished = _OnAllScenesFinished;
    }

    public void PlayCutscene(GameObject prefab, System.Action Callback)
    {
        GameObject cutsceneObject = Instantiate<GameObject>(prefab);
        Cutscene cutscene = cutsceneObject.GetComponent<Cutscene>();

        cutscene.Begin(Callback);
    }

    public void ProcessNext()
    {
        if (_currentIndex < _cutscenes.Count)
        {
            GameObject cutsceneObject = Instantiate<GameObject>(_cutscenes[_currentIndex].gameObject);
            Cutscene cutscene = cutsceneObject.GetComponent<Cutscene>();

            cutscene.Begin(OnCutsceneFinished);
        }
        else if (OnAllScenesFinished != null)
        {
            OnAllScenesFinished.Invoke();
        }
    }

    private void OnCutsceneFinished()
    {
        _currentIndex++;
        ProcessNext();
    }
}
