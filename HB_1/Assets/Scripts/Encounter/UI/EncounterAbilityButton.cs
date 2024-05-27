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

    public void Populate(AbilityID abilityID, Texture2D icon, CharacterComponent character)
    {
        Text_Title.text = GetDisplayString(abilityID);

        Rect rect = new Rect(0, 0, icon.width, icon.height);

        Image_Icon.sprite = Sprite.Create(icon, rect, new Vector2(0.5f, 0.5f));

        _abilityID = abilityID;

        this.interactable = ValidateAbility(abilityID, character);
    }

    public AbilityID GetAbilityID()
    {
        return _abilityID;
    }
}
