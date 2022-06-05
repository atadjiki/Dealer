using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Constants;

public class TimePanel : UIPanel
{
    [SerializeField] private TextMeshProUGUI ButtonText;
    [SerializeField] private TextMeshProUGUI ClockText;
    public override void Build()
    {
        base.Build();
    }

    public override void ShowPanel()
    {
        UpdateButton();
        base.ShowPanel();
    }

    public override void HidePanel()
    {
        base.HidePanel();
    }

    public override void UpdatePanel()
    {
        ClockText.text = TimeManager.Instance.GetDayProgressAsTime();

        base.UpdatePanel();
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

    public void OnModeChanged(State.GameMode Mode)
    {
        UpdateButton();
    }

    public void OnButtonClicked()
    {
        Debug.Log(this.name + " clicked");
        TimeManager.Instance.CycleTimeScale();
        UpdateButton();
    }
}

