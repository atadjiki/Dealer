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
    private Image Image_Icon;

    protected override void Awake()
    {
        Text_Title = GetComponentInChildren<TextMeshProUGUI>();
        Image_Icon = GetComponentInChildren<Image>();
    }

    public void Populate(AbilityID abilityID, Sprite icon, CharacterComponent character)
    {
        Text_Title.text = GetDisplayString(abilityID);

        Image_Icon.sprite = icon;

        _abilityID = abilityID;

        this.interactable = ValidateAbility(abilityID, character);
    }

    public AbilityID GetAbilityID()
    {
        return _abilityID;
    }
}
