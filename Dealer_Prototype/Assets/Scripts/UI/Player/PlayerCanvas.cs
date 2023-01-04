using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Constants;
using GameDelegates;
using TMPro;

public class PlayerCanvas : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI Text_CursorContext;

    private void Start()
    {
        Text_CursorContext.text = string.Empty;
        Global.OnMouseContextChanged += SetPlayerActionContext;
    }

    private void OnDestroy()
    {
        Global.OnMouseContextChanged -= SetPlayerActionContext;
    }

    public void SetPlayerActionContext(Enumerations.MouseContext mouseContext)
    {
        switch (mouseContext)
        {
            case Enumerations.MouseContext.None:
                Text_CursorContext.text = string.Empty;
                break;
            case Enumerations.MouseContext.Door:
                Text_CursorContext.text = "Exit";
                break;
            default:
                Text_CursorContext.text = mouseContext.ToString();
                break;
        }
    }
}
