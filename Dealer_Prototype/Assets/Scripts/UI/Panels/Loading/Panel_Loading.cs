using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine;

public class Panel_Loading : UIPanel
{
    public GameObject SubPanel;

    public override void OnGameModeChanged(State.GameMode GameMode)
    {
        switch(GameMode)
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
