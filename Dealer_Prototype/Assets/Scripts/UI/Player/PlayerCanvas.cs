using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerCanvas : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI Text_CursorContext;

    public enum CursorContext { Move, Interact, None };

    private void Start()
    {
        SetCursorContext(CursorContext.None);

        PlayerComponent.OnPlayerCursorContextChangedDelegate += SetCursorContext;
    }

    public void SetCursorContext(CursorContext context)
    {
        switch (context)
        {
            case CursorContext.Interact:
            case CursorContext.Move:
                Text_CursorContext.text = context.ToString();
                break;

            case CursorContext.None:
                Text_CursorContext.text = string.Empty;
                break;

        }
    }
}
