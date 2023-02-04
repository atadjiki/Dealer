using System.Collections;
using System.Collections.Generic;
using GameDelegates;
using UnityEngine;

public class SafehouseCanvas : MonoBehaviour
{
    private void Awake()
    {
        Global.OnGameStateChanged += Refresh;
        Global.OnToggleUI += OnToggleUI;

        Refresh();
    }

    private void OnDestroy()
    {
        Global.OnGameStateChanged -= Refresh;
        Global.OnToggleUI -= OnToggleUI;
    }

    private void Refresh()
    {
        //Text_Money.text = "$" + GameState.GetMoney();
        //Text_Drugs.text = "" + GameState.GetDrugs();
        //Text_Day.text = "Day " + GameState.GetDay();
    }

    private void OnToggleUI(bool flag)
    {
        this.gameObject.SetActive(flag);
    }
}
