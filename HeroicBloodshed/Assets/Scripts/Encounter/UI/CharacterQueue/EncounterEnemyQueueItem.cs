using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using static Constants;

public class EncounterEnemyQueueItem : EncounterCharacterQueueItem, IPointerClickHandler
{
    [Header("Members")]
    [SerializeField] private Image Image_Portrait;
    [SerializeField] private TextMeshProUGUI Text_Title;
    [SerializeField] private TextMeshProUGUI Text_HP;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_characterComponent.IsAlive())
        {
            _characterComponent.ToggleHighlight(false);
            EncounterManager.Instance.SelectTarget(_characterComponent);
        }
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        if(_characterComponent.IsAlive())
        {
            _characterComponent.ToggleHighlight(true);
            SetHighlight(true);
        }
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        if (_characterComponent.IsAlive())
        {
            _characterComponent.ToggleHighlight(false);
            SetHighlight(false);
        }
    }

    public override void Setup(CharacterComponent character)
    {
        base.Setup(character);

        CharacterID characterID = character.GetID();

        Text_Title.text = Constants.GetDisplayString(characterID);

        Text_HP.text = character.GetHealth() + "/" + character.GetBaseHealth();
    }
}
