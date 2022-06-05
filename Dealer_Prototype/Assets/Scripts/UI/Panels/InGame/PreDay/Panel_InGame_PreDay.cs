using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Panel_InGame_PreDay : UIPanel
{
    [SerializeField] private TextMeshProUGUI Text_Title;
    [SerializeField] private Button Button_Start;

    public override void Build()
    {
        base.Build();
    }

    public override void OnGamePlayModeChanged(State.GamePlayMode GamePlayMode)
    {
        switch(GamePlayMode)
        {
            case State.GamePlayMode.PreDay:
                allowUpdate = true;
                ShowPanel();
                break;
            default:
                allowUpdate = false;
                HidePanel();
                break;
        }
    }

    public override void ShowPanel()
    {
        this.gameObject.SetActive(true);
    }

    public override void HidePanel()
    {
        this.gameObject.SetActive(false);
    }

    public void OnButtonClicked()
    {
        GameStateManager.Instance.ToGamePlayMode(State.GamePlayMode.Day);
    }
}
