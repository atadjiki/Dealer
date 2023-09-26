using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

[Serializable]
public struct EncounterAbilityIconInfo
{
    public AbilityID ID;
    public Sprite Icon;
}

public class EncounterUIAbilitySelect : EncounterUIElement
{
    [SerializeField] protected Transform Container;

    [Header("Icons")]
    [SerializeField] private List<EncounterAbilityIconInfo> IconMap;

    [Header("Prefabs")]
    [SerializeField] protected GameObject Prefab_Item;

    public override void Populate(EncounterModel model)
    {
        CharacterComponent character = model.GetCurrentCharacter();

        foreach (AbilityID abilityID in GetAllowedAbilities(character.GetID()))
        {
            StartCoroutine(GenerateAbilityButton(abilityID, character));
        }
    }

    private IEnumerator GenerateAbilityButton(AbilityID abilityID, CharacterComponent character)
    {
        GameObject ButtonObject = Instantiate(Prefab_Item, Container);
        EncounterAbilityButton abilityButton = ButtonObject.GetComponent<EncounterAbilityButton>();
        yield return new WaitUntil(() => abilityButton != null);
        abilityButton.Populate(abilityID, GetSprite(abilityID), character);
        abilityButton.onClick.AddListener(() => OnAbilityButtonClicked(abilityButton));
    }

    public override void HandleStateUpdate(EncounterState stateID, EncounterModel model)
    {
        switch (stateID)
        {
            case EncounterState.CHOOSE_ACTION:
                {
                    if (model.GetCurrentTeam() == TeamID.Player)
                    {
                        if (!model.IsCurrentTeamCPU())
                        {
                            Populate(model);
                            Show();
                        }
                    }
                    else
                    {
                        Hide();
                    }
                    break;
                }
            default:
                {
                    Hide();
                    break;
                }
        }
    }

    private void OnAbilityButtonClicked(EncounterAbilityButton button)
    {
        if (EncounterManager.Instance != null)
        {
            EncounterManager.Instance.OnAbilitySelected(button.GetAbilityID());
        }
    }

    public override void Hide()
    {
        UIHelper.ClearTransformChildren(Container);

        base.Hide();
    }

    public Sprite GetSprite(AbilityID abilityID)
    {
        foreach(EncounterAbilityIconInfo info in IconMap)
        {
            if(info.ID == abilityID)
            {
                return info.Icon;
            }
        }

        return null;
    }
}
