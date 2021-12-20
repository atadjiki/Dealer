using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class CharacterCanvas : BillboardCanvas
{
    [SerializeField] internal Image Panel;
    [SerializeField] internal TextMeshProUGUI Text_ID;
    [SerializeField] internal TextMeshProUGUI Text_Behavior;

    private void Awake()
    {
        Toggle(false);

        Text_ID.text = "";
        Text_Behavior.text = "";

        if (!DebugManager.Instance.LogCharacter)
        {
            Panel.color = Color.clear;
        }
    }

    public void Set_Text_ID(string text)
    {
        if(Text_ID != null && DebugManager.Instance.LogCharacter)
            Text_ID.text = text;
    }

    public void Set_Text_Mode(string text)
    {
        if (Text_Behavior != null && DebugManager.Instance.LogCharacter)
            Text_Behavior.text = text;
    }
}
