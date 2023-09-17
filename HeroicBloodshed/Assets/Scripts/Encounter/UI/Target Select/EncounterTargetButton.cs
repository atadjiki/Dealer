using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static Constants;
using UnityEngine.EventSystems;

public class EncounterTargetButton : Button, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private TextMeshProUGUI Text_Name;

    private CharacterComponent _character;

    protected override void Awake()
    {
        Text_Name = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void Populate(CharacterComponent characterComponent)
    {
        _character = characterComponent;

        Text_Name.text = GetDisplayString(_character.GetID());

        this.interactable = characterComponent.IsAlive();
    }

    public override void OnPointerEnter(PointerEventData pointerEventData)
    {
        base.OnPointerEnter(pointerEventData);

        if (_character != null && _character.IsAlive())
        {
            _character.ToggleHighlight(true);
            EncounterManager.Instance.FollowCharacter(_character);
        }
    }

    public override void OnPointerExit(PointerEventData pointerEventData)
    {
        base.OnPointerExit(pointerEventData);

        if (_character != null)
        {
            _character.ToggleHighlight(false);
            EncounterManager.Instance.UnfollowCharacter();
        }
    }

    public CharacterComponent GetCharacter()
    {
        return _character;
    }
}
