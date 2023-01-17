using System.Collections;
using UnityEngine;
using Constants;
using System.Collections.Generic;

[System.Serializable]
public class CutsceneCharacterData
{
    public Enumerations.CharacterID ID;
    public Enumerations.CharacterModelID Model;
    public Transform SpawnLocation;
}

public class Cutscene : MonoBehaviour
{
    [SerializeField] private CutsceneNode _root;

    [SerializeField] private List<CutsceneCharacterData> CharacterData;

    private List<CutsceneCharacterComponent> Characters = new List<CutsceneCharacterComponent>();

    private CutsceneNode _currentNode;

    private System.Action OnCutsceneFinished;

    private TransitionPanel transitionPanel;

    public void Begin(System.Action _OnCutsceneFinished)
    {
        OnCutsceneFinished = _OnCutsceneFinished;

        _currentNode = _root;

        transitionPanel = UIUtility.RequestTransitionScreen(true);

        StartCoroutine(Coroutine_Begin());
    }

    private IEnumerator Coroutine_Begin()
    {
        foreach (CutsceneCharacterData data in CharacterData)
        {
            CutsceneCharacterComponent characterComponent = CutsceneHelper.SpawnCutsceneCharacter(this, data);

            yield return new WaitUntil(() => characterComponent != null);

            if (characterComponent != null)
            {
                Characters.Add(characterComponent);
            }

            yield return new WaitForSeconds(1f);

            transitionPanel.Toggle(false);
        }

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
            foreach (CutsceneEvent cutsceneEvent in _currentNode.GetPreEvents().ToList())
            {
                CutsceneHelper.ProcessCutsceneEvent(this, cutsceneEvent);
            }

            if (_currentNode.DoFadeIn())
            {
                transitionPanel.Toggle(true, _currentNode.GetFadeIn());
                yield return new WaitForSeconds(_currentNode.GetFadeIn());
            }

            _currentNode.Setup(this, OnNodeComplete);
        }
        else
        {
            transitionPanel.ToggleAndDestroy(false, 1.0f);
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
                transitionPanel.Toggle(true, _currentNode.GetFadeOut());
                yield return new WaitForSeconds(_currentNode.GetFadeOut());
            }

            foreach (CutsceneEvent cutsceneEvent in _currentNode.GetPostEvents().ToList())
            {
                CutsceneHelper.ProcessCutsceneEvent(this, cutsceneEvent);
            }
        }

        _currentNode = _currentNode.GetNext();

        ProcessCurrentNode();
    }

    public List<CutsceneCharacterComponent> GetCharacters()
    {
        return Characters;
    }

    public CutsceneCharacterComponent FindCharacter(Enumerations.CharacterID characterID)
    {
        foreach(CutsceneCharacterComponent characterComponent in Characters)
        {
            if(characterComponent.CharacterID == characterID)
            {
                return characterComponent;
            }
        }

        return null;
    }
}

