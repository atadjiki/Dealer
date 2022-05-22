using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class MoneyPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI Text_Money;

    private void Awake()
    {
        SetMoney(GameStateManager.Instance.state.money);

        GameStateManager.Instance.onStateChanged += OnStateChanged;
    }

    private void OnStateChanged(GameState state)
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

