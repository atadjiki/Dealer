using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Party_InGame_Day_Party_ListItem : MonoBehaviour
{
    private TextMeshProUGUI textBox;

    private void Awake()
    {
        textBox = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void SetText(string text)
    {
        if(textBox != null)
        {
            textBox.text = text;
        }
    }

    public void SetTextColor(Color color)
    {
        if(textBox != null)
        {
            textBox.color = color;
        }
    }

    public void ToggleVisiblity(bool flag)
    {
        if(textBox != null)
        {
            textBox.enabled = flag;
        }

        this.gameObject.SetActive(flag);
    }

    public void OnClick()
    {
        Debug.Log("clicked " + textBox.text);
    }
}
