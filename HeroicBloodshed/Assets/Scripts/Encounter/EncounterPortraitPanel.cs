using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static Constants;

public class EncounterPortraitPanel : MonoBehaviour
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

    public void Setup(CharacterComponent character)
    {
        CharacterID characterID = character.GetID();
        TeamID team = GetTeamByID(characterID);
        Text_Name.text = characterID.ToString().Trim().ToLower();

        Image_Portrait.color = GetColorByTeam(team);

        Panel_Dead.SetActive(character.IsDead());
    }
}
