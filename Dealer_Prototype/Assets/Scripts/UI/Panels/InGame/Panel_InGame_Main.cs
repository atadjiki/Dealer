using Constants;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Panel_InGame_Main : UIPanel
{
    public GameObject SubPanel;

    public override void OnGameModeChanged(State.GameMode GameMode)
    {
        switch (GameMode)
        {
            case State.GameMode.Loading:
                ShowPanel();
                allowUpdate = true;
                break;
            default:
                HidePanel();
                allowUpdate = false;
                break;
        }
    }

    public override void ShowPanel()
    {
        SubPanel.gameObject.SetActive(true);

        base.ShowPanel();
    }

    public override void HidePanel()
    {
        SubPanel.gameObject.SetActive(false);
        base.HidePanel();
    }
}
