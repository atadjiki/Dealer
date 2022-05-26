using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class Panel_InGame_Money : UIPanel
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

    public void OnStateChanged(GameState state)
    {
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

