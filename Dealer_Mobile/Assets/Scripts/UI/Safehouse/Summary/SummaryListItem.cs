using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SummaryListItem : ListItem
{
    [SerializeField] private TextMeshProUGUI Text_Info;

    public void Setup(string text)
    {
        Text_Info.text = text;
    }
}
