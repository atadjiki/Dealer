using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EncounterWinDetailItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI Text_Detail;
    [SerializeField] private TextMeshProUGUI Text_Value;

    public void Populate(string detail, string value)
    {
        Text_Detail.text = detail.Trim().ToLower();
        Text_Value.text = value.Trim().ToLower();
    }
}
