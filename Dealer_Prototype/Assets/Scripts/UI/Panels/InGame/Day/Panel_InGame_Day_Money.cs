using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using Constants;

public class Panel_InGame_Day_Money : UIPanel
{ 
    [SerializeField] private TextMeshProUGUI Text_Money;

    public override void Build()
    {
        base.Build();
    }

    public override void ShowPanel()
    {
        Text_Money.enabled = true;
    }

    public override void HidePanel()
    {
        Text_Money.enabled = false;
    }

    public override void OnGamePlayModeChanged(State.GamePlayMode GamePlayMode)
    {
        switch(GamePlayMode)
        {
            case State.GamePlayMode.Day:
                allowUpdate = true;
                ShowPanel();
                break;
            default:
                allowUpdate = false;
                HidePanel();
                break;
        }
    }

    public override void OnGameStateChanged(GameState state)
    {
        if(Text_Money != null && state != null)
            SetMoney(state.money);
    }

    public void SetMoney(float money)
    {
        if (money > 0)
        {
            string moneyString = string.Format("{0:#.00}", money);
            Text_Money.text = "$" + moneyString;
        }
        else
        {
            Text_Money.text = "$" + 0;
        }
    }
}

