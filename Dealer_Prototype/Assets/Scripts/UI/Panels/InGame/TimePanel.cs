using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
            case TimeManager.TimeMode.Paused:
                ButtonText.text = "paused";
                break;
            case TimeManager.TimeMode.Normal:
                ButtonText.text = "normal";
                break;
            case TimeManager.TimeMode.Slow:
                ButtonText.text = "slow";
                break;
            case TimeManager.TimeMode.Fast:
                ButtonText.text = "fast";
                break;
            case TimeManager.TimeMode.VeryFast:
                ButtonText.text = "fastest";
                break;
        }
    }

    public void OnModeChanged(GameStateManager.Mode Mode)
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

