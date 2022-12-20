using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Constants;
using TMPro;

public class PlayerCanvas : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI Text_CursorContext;

    private void Start()
    {
         PlayerComponent.OnPendingActionChangedDelegate += SetPlayerActionContext;
    }

    private void OnDestroy()
    {
        PlayerComponent.OnPendingActionChangedDelegate -= SetPlayerActionContext;
    }

    public void SetPlayerActionContext(Enumerations.CharacterAction action)
    {
        switch (action)
        {
            case Enumerations.CharacterAction.None:
                Text_CursorContext.text = string.Empty;
                break;
            default:
                Text_CursorContext.text = action.ToString();
                break;
        }
    }
}
