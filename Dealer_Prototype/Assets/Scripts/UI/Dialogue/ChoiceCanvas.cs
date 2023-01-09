using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using GameDelegates;
using Constants;

public class ChoiceCanvas : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI Text_Prompt;

    [SerializeField] Button Button_A;
    [SerializeField] Button Button_B;
    [SerializeField] Button Button_C;
    [SerializeField] Button Button_D;

    public ChoiceSelected onChoiceSelected;

    public void Setup(string prompt, string A, string B, string C, string D)
    {
        Text_Prompt.text = prompt;

        SetupButton(Button_A, A, 0);
        SetupButton(Button_B, B, 1);
        SetupButton(Button_C, C, 2);
        SetupButton(Button_D, D, 3);
    }

    private void SetupButton(Button button, string text, int index)
    {
        button.GetComponentInChildren<TextMeshProUGUI>().text = text;
        button.enabled = (text != null);
        button.onClick.AddListener(() => OnButtonPressed(index) );
    }

    private void OnButtonPressed(int index)
    {
        onChoiceSelected(index);
        Destroy(this.gameObject);
    }
}
