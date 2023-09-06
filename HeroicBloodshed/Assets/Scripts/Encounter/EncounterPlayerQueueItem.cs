using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static Constants;
using UnityEngine.EventSystems;

public class EncounterPlayerQueueItem: EncounterCharacterQueueItem
{
    [Header("Members")]
    [SerializeField] private TextMeshProUGUI Text_Name;
    [SerializeField] private Image Image_Portrait;

    public override void Setup(CharacterComponent character)
    {
        base.Setup(character);

        CharacterID characterID = character.GetID();
        TeamID team = GetTeamByID(characterID);
        Text_Name.text = GetDisplayString(characterID);

        Image_Portrait.color = GetColorByTeam(team);
    }
}
