using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Constants;

public class CharacterCombatCanvas : MonoBehaviour
{
    [SerializeField] private GameObject Panel_Main;
    [SerializeField] private TextMeshProUGUI Text_Position;
    [SerializeField] private TextMeshProUGUI Text_Name;
    [SerializeField] private GameObject HealthBar;

    public void Refresh(int position, CharacterConstants.Team team, CharacterData data)
    {
        Text_Position.text = "1";
        Text_Name.text = team + " " + data.type;

        SetHealthBar(data.health);
    }

    private void SetHealthBar(int value)
    {
        value = Mathf.Clamp(value, 0, 9);
        for(int i = 0; i < HealthBar.transform.childCount; i++)
        {
            if(i > value)
            {
                HealthBar.transform.GetChild(i).gameObject.SetActive(false);
            }
            else
            {
                HealthBar.transform.GetChild(i).gameObject.SetActive(true);
            }
        }
    }

    public void Toggle(bool flag)
    {
        Panel_Main.gameObject.SetActive(flag);
    }
}
