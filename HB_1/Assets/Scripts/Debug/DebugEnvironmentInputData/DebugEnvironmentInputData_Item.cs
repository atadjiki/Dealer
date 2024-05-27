using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DebugEnvironmentInputData_Item : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI Text_Label;
    [SerializeField] private TextMeshProUGUI Text_Value;

    public void Setup(string label, string value)
    {
        Text_Label.text = label;
        Text_Value.text = value;
    }
}
