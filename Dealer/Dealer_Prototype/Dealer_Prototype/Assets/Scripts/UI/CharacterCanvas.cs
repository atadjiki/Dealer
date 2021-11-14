using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class CharacterCanvas : BillboardCanvas
{
    [SerializeField] internal TextMeshProUGUI Text_ID;

    public void Set_Text_ID(string text)
    {
        if(Text_ID != null)
            Text_ID.text = text;
    }
}
