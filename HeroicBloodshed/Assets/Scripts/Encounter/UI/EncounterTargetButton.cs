using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static Constants;
using UnityEngine.EventSystems;

public class EncounterTargetButton : Button, IPointerEnterHandler, IPointerExitHandler
{
    private CharacterComponent _character;

    public void Populate(CharacterComponent characterComponent)
    {
        _character = characterComponent;

        this.interactable = characterComponent.IsAlive();
    }

    public override void OnPointerEnter(PointerEventData pointerEventData)
    {
        base.OnPointerEnter(pointerEventData);

        if (_character != null && _character.IsAlive())
        {
            _character.ToggleHighlight(true);

            //follow character
            EncounterManager.Instance.FollowCharacter(_character);

            //rotate caster towards target
            EncounterManager.Instance.OnEnemyHighlighted(_character);
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
