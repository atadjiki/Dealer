using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static Constants;

public class EncounterAbilityButton : Button
{
    private TextMeshProUGUI Text_Title;
    private AbilityID _abilityID;

    protected override void Awake()
    {
        Text_Title = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void Populate(AbilityID abilityID)
    {
        Text_Title.text = GetDisplayString(abilityID);
        _abilityID = abilityID;
    }

    public AbilityID GetAbilityID()
    {
        return _abilityID;
    }
}
