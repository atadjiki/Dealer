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

    [SerializeField] private List<Image> HealthbarItems;

    private CharacterComponent _character;

    private RectTransform _rectTransform;

    public void Setup(CharacterComponent character)
    {
        _character = character;
        _rectTransform = Panel_Backing.GetComponent<RectTransform>();

        StartCoroutine(Coroutine_Update());
    }

    private IEnumerator Coroutine_Update()
    {
        while(_character.IsAlive())
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(_character.GetOverheadAnchor().transform.position);

            _rectTransform.position = screenPos;

            float health = _character.GetHealth();
            float baseHealth = _character.GetBaseHealth();

            float total = HealthbarItems.Count;

            int threshold = Mathf.RoundToInt(total * health / baseHealth);

            for (int i = 0; i < HealthbarItems.Count; i++)
            {
                HealthbarItems[i].enabled = (i < threshold);
            }

            Text_Health.text = health + "/" + baseHealth;
            Text_Name.text = GetDisplayString(_character.GetID());

            Panel_Backing.color = GetColorByTeam(_character.GetTeam(), 0.3f);
            Panel_Backing.gameObject.SetActive(true);

            yield return null;
        }

        Panel_Backing.gameObject.SetActive(false);
    }
}
