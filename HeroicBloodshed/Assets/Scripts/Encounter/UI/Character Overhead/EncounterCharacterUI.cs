using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static Constants;

public class EncounterCharacterUI : MonoBehaviour
{
    [SerializeField] private Image Panel_Backing;

    [SerializeField] private TextMeshProUGUI Text_Name;

    [SerializeField] private TextMeshProUGUI Text_Health;

    [SerializeField] private List<GameObject> HealthbarItems;

    public void Populate(CharacterComponent character)
    {
        int health = character.GetHealth();
        int baseHealth = character.GetBaseHealth();

        int itemTotal = HealthbarItems.Count;
        int itemCount = itemTotal * (health / baseHealth);

        for (int i = 0; i < itemTotal -1; i++)
        {
            HealthbarItems[i].SetActive(i <= itemCount);
        }

        Text_Health.text = health + "/" + baseHealth;
        Text_Name.text = GetDisplayString(character.GetID());

        Panel_Backing.color = GetColorByTeam(character.GetTeam(), 0.3f);
        Panel_Backing.gameObject.SetActive(character.IsAlive());
    }
}
