using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Constants;

public class Panel_InGame_Day_Time : UIPanel
{

    [SerializeField] private TextMeshProUGUI ButtonText;
    [SerializeField] private TextMeshProUGUI ClockText;

    public override void OnGamePlayModeChanged(State.GamePlayMode GamePlayMode)
    {
        switch(GamePlayMode)
        {
            case State.GamePlayMode.Day:
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
        ButtonText.enabled = true;
        ClockText.enabled = true;

        UpdateButton();
    }

    public override void HidePanel()
    {
        ButtonText.text = "";
        ClockText.text = "";

        ButtonText.enabled = false;
        ClockText.enabled = false;
    }

    public override void UpdatePanel()
    {
        if(allowUpdate)
        {
            if (ClockText.enabled)
            {
                ClockText.text = TimeManager.Instance.GetDayProgressAsTime();
            }
        }
    }

    private void UpdateButton()
    {
        switch(TimeManager.Instance.GetTimeMode())
        {
            case State.TimeMode.Paused:
                ButtonText.text = "paused";
                break;
            case State.TimeMode.Normal:
                ButtonText.text = "normal";
                break;
            case State.TimeMode.Slow:
                ButtonText.text = "slow";
                break;
            case State.TimeMode.Fast:
                ButtonText.text = "fast";
                break;
            case State.TimeMode.VeryFast:
                ButtonText.text = "fastest";
                break;
        }
    }

    public void OnButtonClicked()
    {
        Debug.Log(this.name + " clicked");
        TimeManager.Instance.CycleTimeScale();
        UpdateButton();
    }
}

