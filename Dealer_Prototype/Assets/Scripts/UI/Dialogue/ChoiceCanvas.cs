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

    public void Setup(ChoiceActionData Data)
    {
        Text_Prompt.text = Data.Prompt;

        SetupButton(Button_A, Data.Text_A, 0);
        SetupButton(Button_B, Data.Text_B, 1);
        SetupButton(Button_C, Data.Text_C, 2);
        SetupButton(Button_D, Data.Text_D, 3);
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
