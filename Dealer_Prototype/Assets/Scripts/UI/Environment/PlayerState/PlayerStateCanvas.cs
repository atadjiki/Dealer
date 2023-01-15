using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using GameDelegates;

public class PlayerStateCanvas : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI Text_Money;
    [SerializeField] private TextMeshProUGUI Text_Drugs;
    [SerializeField] private TextMeshProUGUI Text_Day;

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
        Text_Money.text = "$" + GameState.GetMoney();
        Text_Drugs.text = "" + GameState.GetDrugs();
        Text_Day.text = "Day " + GameState.GetDay();
    }

    private void OnToggleUI(bool flag)
    {
        this.gameObject.SetActive(flag);
    }
}

