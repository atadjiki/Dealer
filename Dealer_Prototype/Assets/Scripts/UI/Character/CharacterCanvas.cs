using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CharacterCanvas : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI Text_Name;
    [SerializeField] private TextMeshProUGUI Text_Level;
    [SerializeField] private GameObject Group_Healthbar;

    public void SetName(string text)
    {
        ToggleName(true);
        Text_Name.text = text;
    }

    public void SetLevel(int level)
    {
        ToggleLevel(true);
        Text_Level.text = level.ToString();
    }

    public void ToggleName(bool flag)
    {
        Text_Name.gameObject.SetActive(flag);
    }

    public void ToggleLevel(bool flag)
    {
        Text_Level.gameObject.SetActive(flag);
    }

    public void ToggleHealthBar(bool flag)
    {
        Group_Healthbar.SetActive(flag);
    }

    public void Toggle(bool flag)
    {
        this.gameObject.SetActive(flag);
    }
}
