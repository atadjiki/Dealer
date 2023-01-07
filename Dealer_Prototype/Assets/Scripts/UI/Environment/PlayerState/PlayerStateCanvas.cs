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

        SaveData saveData = GameState.Load();

        Refresh(saveData);
    }

    private void OnDestroy()
    {
        Global.OnGameStateChanged -= Refresh;
        Global.OnToggleUI -= OnToggleUI;
    }

    private void Refresh(SaveData _data)
    {
        Text_Money.text = "$" + _data.Money.ToString();
        Text_Drugs.text = _data.Drugs.ToString();
        Text_Day.text = "Day " + _data.Day.ToString();
    }

    private void OnToggleUI(bool flag)
    {
        this.gameObject.SetActive(flag);
    }
}
