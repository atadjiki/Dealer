using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    private static UIManager _instance;

    public static UIManager Instance { get { return _instance; } }

    public GameObject InGamePanel;
    public TextMeshProUGUI InGame_ActionText;

    public GameObject DialoguePanel;
    public TextMeshProUGUI Dialogue_SpeechText;

    private List<GameObject> Panels;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }

        Build();
    }

    private void Build()
    {
        InGame_ActionText.text = "";

        Panels = new List<GameObject>() { InGamePanel, DialoguePanel };

        SetAll(false);
        ToggleInGame(true);
    }

    public void SetActionText(string text)
    {
        InGame_ActionText.text = text;
    }

    public void ToggleInGame(bool flag)
    {
        InGamePanel.SetActive(flag);
    }

    private void SetAll(bool flag)
    {
        foreach(GameObject obj in Panels)
        {
            obj.SetActive(flag);
        }
    }

    public void DisplaySpeech(string text, float duration)
    {
        StopAllCoroutines();
        StartCoroutine(DoDisplaySpeech(text, duration));
    }

    IEnumerator DoDisplaySpeech(string text, float duration)
    {
        InGamePanel.SetActive(false);
        Dialogue_SpeechText.text = text;
        DialoguePanel.SetActive(true);

        yield return new WaitForSeconds(duration);

        DialoguePanel.SetActive(false);
        Dialogue_SpeechText.text = "";
        InGamePanel.SetActive(true);
    }
}
