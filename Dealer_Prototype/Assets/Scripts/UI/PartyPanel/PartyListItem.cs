using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PartyListItem : MonoBehaviour
{
    private TextMeshProUGUI textBox;

    private void Awake()
    {
        textBox = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void SetText(string text)
    {
        textBox.text = text;
    }

    public void SetTextColor(Color color)
    {
        textBox.color = color;
    }

    public void ToggleVisiblity(bool flag)
    {
        this.gameObject.SetActive(flag);
    }

    public void OnClick()
    {
        Debug.Log("clicked " + textBox.text);
    }
}
