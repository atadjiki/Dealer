using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Constants;
using System;
using UnityEngine.EventSystems;

public class UIPanel_DebugMenu_LoadEnvironment : UIPanel
{
    [Header("Load Environment")]
    [SerializeField] private Dropdown Dropdown_LoadEnvironment;
    [SerializeField] private Button Button_LoadEnvironment;

    private void Awake()
    {
        if(Dropdown_LoadEnvironment == null || Button_LoadEnvironment == null)
        {
            Destroy(this.gameObject);
        }

        Dropdown_LoadEnvironment.AddOptions(new List<string>(Enum.GetNames(typeof(Enumerations.Environment))));

    }

    public void OnButton_LoadEnvironment()
    {
        string value = Dropdown_LoadEnvironment.options[Dropdown_LoadEnvironment.value].text;
        Debug.Log(value);

        Enumerations.Environment environmentID = (Enumerations.Environment) Enum.Parse(typeof(Enumerations.Environment), value);

        GameStateManager.Instance.SetEnvironment(environmentID);
    }

    public override void PerformUpdate()
    {
        base.PerformUpdate();
    }
}
