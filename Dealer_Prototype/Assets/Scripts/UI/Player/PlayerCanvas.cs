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
         Global.OnPendingCommandChanged += SetPlayerActionContext;
    }

    private void OnDestroy()
    {
        Global.OnPendingCommandChanged -= SetPlayerActionContext;
    }

    public void SetPlayerActionContext(Enumerations.CommandType action)
    {
        switch (action)
        {
            case Enumerations.CommandType.None:
                Text_CursorContext.text = string.Empty;
                break;
            default:
                Text_CursorContext.text = action.ToString();
                break;
        }
    }
}
