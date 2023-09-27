using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static Constants;

public class EncounterCharacterUI : MonoBehaviour, ICharacterEventReceiver
{
    [SerializeField] private Image Panel_Backing;

    [SerializeField] private TextMeshProUGUI Text_Name;

    [SerializeField] private TextMeshProUGUI Text_Health;

    [SerializeField] private List<Image> HealthbarItems;

    [SerializeField] private List<Image> DamagePreviewItems;

    private CharacterComponent _character;

    private RectTransform _rectTransform;

    private bool _canReceive = true;

    public void Setup(CharacterComponent character)
    {
        _character = character;
        _rectTransform = Panel_Backing.GetComponent<RectTransform>();

        Text_Name.text = GetDisplayString(_character.GetID());

        Panel_Backing.color = GetColorByTeam(_character.GetTeam(), 0.3f);
        Panel_Backing.gameObject.SetActive(true);

        UpdateHealthBar();
    }

    private void FixedUpdate()
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(_character.GetOverheadAnchor().transform.position);

        _rectTransform.position = screenPos;
    }

    public void HandleEvent(object eventData, CharacterEvent characterEvent)
    {
        switch (characterEvent)
        {
            case CharacterEvent.TARGETED:
                HandleEvent_Targeted((DamageInfo)eventData);
                break;
            case CharacterEvent.UNTARGETED:
                HandleEvent_Untargeted();
                break;
            case CharacterEvent.DAMAGE:
                HandleEvent_Damage();
                break;
            case CharacterEvent.KILLED:
                HandleEvent_Killed();
                break;
            default:
                break;
        }
    }

    private void HandleEvent_Targeted(DamageInfo damageInfo)
    {
        float currentHealth = damageInfo.target.GetHealth();
        float damagedHealth = currentHealth - damageInfo.ActualDamage;

        float total = DamagePreviewItems.Count;

        int threshold = Mathf.RoundToInt(total * damagedHealth / currentHealth);

        for (int i = 0; i < DamagePreviewItems.Count; i++)
        {
            DamagePreviewItems[DamagePreviewItems.Count - i - 1].enabled = (i < threshold);
        }
    }

    private void HandleEvent_Untargeted()
    {
        for (int i = 0; i < DamagePreviewItems.Count; i++)
        {
            DamagePreviewItems[i].enabled = false;
        }
    }

    private void HandleEvent_Damage()
    {
        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        float health = _character.GetHealth();
        float baseHealth = _character.GetBaseHealth();

        float total = HealthbarItems.Count;

        int threshold = Mathf.RoundToInt(total * health / baseHealth);

        for (int i = 0; i < HealthbarItems.Count; i++)
        {
            HealthbarItems[i].enabled = (i < threshold);
        }

        Text_Health.text = health + "/" + baseHealth;
    }

    private void HandleEvent_Killed()
    {
        Panel_Backing.gameObject.SetActive(false);
    }

    public bool CanReceiveCharacterEvents()
    {
        return _canReceive;
    }
}
