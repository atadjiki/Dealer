using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using GameDelegates;
using Constants;

public class ChoiceCanvas : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI Text_Prompt;

    [SerializeField] private GameObject Choices_Panel;

    public ChoiceSelected onChoiceSelected;

    public void Setup(ChoiceActionData Data)
    {
        Text_Prompt.text = Data.MainText;

        for(int i = 0; i < Data.Choices.Count; i++)
        {
            SetupButton(Data.Choices[i].Text, i);
        }
    }

    private void SetupButton(string text, int index)
    {
        if (text != null)
        {
            GameObject buttonObject = Instantiate<GameObject>(PrefabLibrary.GetChoiceButton(), Choices_Panel.transform);
            Button button = buttonObject.GetComponent<Button>();
            button.GetComponentInChildren<TextMeshProUGUI>().text = text;
            button.interactable = text.Length > 0;

            button.onClick.AddListener(() => OnButtonPressed(index));
        }
    }

    private void OnButtonPressed(int index)
    {
        onChoiceSelected(index);
        Destroy(this.gameObject);
    }
}
