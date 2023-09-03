using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static Constants;
using UnityEngine.EventSystems;

public class EncounterPortraitPanel : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private TextMeshProUGUI Text_Name;
    [SerializeField] private Image Image_Portrait;
    [SerializeField] private GameObject Panel_Active;
    [SerializeField] private GameObject Panel_Inactive;
    [SerializeField] private GameObject Panel_Dead;

    private CharacterComponent _characterComponent;

    private void OnEnable()
    {
        Panel_Active.SetActive(false);
        Panel_Inactive.SetActive(false);
        Panel_Dead.SetActive(false);
    }

    public void Setup(CharacterComponent character)
    {
        _characterComponent = character;

        CharacterID characterID = character.GetID();
        TeamID team = GetTeamByID(characterID);
        Text_Name.text = characterID.ToString().Trim().ToLower();

        Image_Portrait.color = GetColorByTeam(team);

        Panel_Dead.SetActive(character.IsDead());
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("on mouse over");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("on mouse exit");
    }
}
