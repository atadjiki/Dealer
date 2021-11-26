using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class CharacterCanvas : BillboardCanvas
{
    [SerializeField] internal TextMeshProUGUI Text_ID;
    [SerializeField] internal TextMeshProUGUI Text_State;
    [SerializeField] internal TextMeshProUGUI Text_Behavior;

    private void Awake()
    {
        Toggle(false);
    }

    public void Set_Text_ID(string text)
    {
        if(Text_ID != null)
            Text_ID.text = text;
    }

    public void Set_Text_State(string text)
    {
        if (Text_State != null)
            Text_State.text = text;
    }

    public void Set_Text_Mode(string text)
    {
        if (Text_Behavior != null)
            Text_Behavior.text = text;
    }
}
