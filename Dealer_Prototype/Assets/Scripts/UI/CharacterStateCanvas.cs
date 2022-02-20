using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CharacterStateCanvas : MonoBehaviour
{
    public TextMeshProUGUI Text_State;

    public void SetText_State(string text)
    {
        if(Text_State != null)
        {
            Text_State.text = text.ToLower().Trim();
        }
    }
}
