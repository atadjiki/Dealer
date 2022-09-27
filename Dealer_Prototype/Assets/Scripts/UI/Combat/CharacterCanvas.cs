using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CharacterCanvas : UIPanel
{
    [SerializeField] private TextMeshProUGUI Text_Name;

    public void SetName(string text)
    {
        Text_Name.text = text;
    }   
}
