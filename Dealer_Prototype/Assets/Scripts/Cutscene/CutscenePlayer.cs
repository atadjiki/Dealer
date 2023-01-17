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
        GameObject cutsceneInstance = Instantiate(prefab);
        Cutscene cutscene = cutsceneInstance.GetComponent<Cutscene>();

        StartCoroutine(Coroutine_PlayCutscene(cutscene, Callback));
    }

    private IEnumerator Coroutine_PlayCutscene(Cutscene cutscene, System.Action Callback)
    {
        UIUtility.FadeToBlack(0);
        yield return new WaitForSeconds(1.0f);
        cutscene.Begin(Callback);
    }

    public void ProcessNext()
    {
        if (_currentIndex < _cutscenes.Count)
        {
            PlayCutscene(_cutscenes[_currentIndex].gameObject, OnCutsceneFinished);
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