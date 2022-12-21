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

    public void SetPlayerActionContext(Enumerations.CharacterCommand action)
    {
        switch (action)
        {
            case Enumerations.CharacterCommand.None:
                Text_CursorContext.text = string.Empty;
                break;
            default:
                Text_CursorContext.text = action.ToString();
                break;
        }
    }
}
