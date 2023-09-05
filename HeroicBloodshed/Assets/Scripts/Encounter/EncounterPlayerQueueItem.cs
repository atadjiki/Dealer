using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static Constants;
using UnityEngine.EventSystems;

public class EncounterPlayerQueueItem: EncounterCharacterQueueItem
{
    [SerializeField] private TextMeshProUGUI Text_Name;
    [SerializeField] private Image Image_Portrait;
    [SerializeField] private GameObject Panel_Active;
    [SerializeField] private GameObject Panel_Inactive;
    [SerializeField] private GameObject Panel_Dead;

    private void OnEnable()
    {
        Panel_Active.SetActive(false);
        Panel_Inactive.SetActive(false);
        Panel_Dead.SetActive(false);
    }

    public override void Setup(CharacterComponent character)
    {
        base.Setup(character);

        CharacterID characterID = character.GetID();
        TeamID team = GetTeamByID(characterID);
        Text_Name.text = GetDisplayString(characterID);

        Image_Portrait.color = GetColorByTeam(team);
    }

    public override void SetActive()
    {
        base.SetActive();

        Panel_Dead.SetActive(false);
        Panel_Active.SetActive(true);
        Panel_Inactive.SetActive(false);
    }

    public override void SetInactive()
    {
        base.SetInactive();

        Panel_Dead.SetActive(false);
        Panel_Active.SetActive(false);
        Panel_Inactive.SetActive(true);
    }

    public override void SetDead()
    {
        base.SetDead();

        Panel_Dead.SetActive(true);
        Panel_Active.SetActive(false);
        Panel_Inactive.SetActive(false);
    }
}
