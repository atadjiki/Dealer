using System.Collections;
using System.Collections.Generic;
using GameDelegates;
using Constants;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class SafehouseCanvas : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI Text_Money;
    [SerializeField] private TextMeshProUGUI Text_Day;

    [SerializeField] private Button Button_Settings;
    [SerializeField] private Button Button_Stash;
    [SerializeField] private Button Button_Phone;
    [SerializeField] private Button Button_Leave;

    private void HandleButtonClick(Button button)
    {
        Debug.Log("Clicked: " + button.name);
    }

    private void Awake()
    {
        Global.OnGameStateChanged += Refresh;
     //   Global.OnToggleUI += OnToggleUI;

        Button_Settings.onClick.AddListener(delegate ()
        {
            HandleButtonClick(Button_Settings);
        });

        Button_Stash.onClick.AddListener(delegate ()
        {
            HandleButtonClick(Button_Stash);
        });

        Button_Phone.onClick.AddListener(delegate ()
        {
            HandleButtonClick(Button_Phone);
        });

        Button_Leave.onClick.AddListener(delegate ()
        {
            HandleButtonClick(Button_Leave);
        });

        Refresh();
    }

    private void OnDestroy()
    {
        Global.OnGameStateChanged -= Refresh;
       // Global.OnToggleUI -= OnToggleUI;
    }

    private void Refresh()
    {
        Text_Money.text = "$" + GameState.GetSafehouseItem(Enumerations.InventoryID.MONEY);
        Text_Day.text = "Day " + GameState.GetDay();
    }

    private void OnToggleUI(bool flag)
    {
        this.gameObject.SetActive(flag);
    }
}
