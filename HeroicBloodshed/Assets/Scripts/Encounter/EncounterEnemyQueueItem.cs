using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using static Constants;

public class EncounterEnemyQueueItem : EncounterCharacterQueueItem
{
    [SerializeField] private Image Image_Portrait;
    [SerializeField] private TextMeshProUGUI Text_Title;
    [SerializeField] private TextMeshProUGUI Text_HP;

    public override void Setup(CharacterComponent character)
    {
        base.Setup(character);

        CharacterID characterID = character.GetID();

        Text_Title.text = Constants.GetDisplayString(characterID);

        Text_HP.text = character.GetHealth() + "/" + character.GetBaseHealth();
    }
}
