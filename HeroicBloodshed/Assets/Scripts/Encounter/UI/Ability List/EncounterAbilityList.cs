using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

public class EncounterAbilityList : EncounterCanvasItemContainer
{
    public override void Populate(EncounterModel model)
    {
        CharacterComponent character = model.GetCurrentCharacter();

        foreach (AbilityID abilityID in GetAllowedAbilities(character.GetID()))
        {
            GameObject ButtonObject = Instantiate(Prefab_Item, Container);
            EncounterAbilityButton abilityButton = ButtonObject.GetComponent<EncounterAbilityButton>();
            abilityButton.Populate(abilityID);
            abilityButton.onClick.AddListener(() => OnAbilityButtonClicked(abilityButton));
        }
    }

    private void OnAbilityButtonClicked(EncounterAbilityButton button)
    {
        if (EncounterManager.Instance != null)
        {
            EncounterManager.Instance.SelectAbility(button.GetAbilityID());
        }
    }
}
