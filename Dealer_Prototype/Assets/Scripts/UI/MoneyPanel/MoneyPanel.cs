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
        SetMoney(0);
    }

    public void SetMoney(int money)
    {
        if (money > 0)
        {
            string moneyString = string.Format("{0:#.00}", money / 100);
            Text_Money.text = "$" + moneyString;
        }
        else
        {
            Text_Money.text = "$" + 0;
        }
    }
}
