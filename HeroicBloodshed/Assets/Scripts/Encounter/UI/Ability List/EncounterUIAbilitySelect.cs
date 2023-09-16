using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

public class EncounterUIAbilitySelect : EncounterUIElement
{
    [SerializeField] protected Transform Container;

    [SerializeField] protected GameObject Prefab_Item;

    public override void Populate(EncounterModel model)
    {
        CharacterComponent character = model.GetCurrentCharacter();

        foreach (AbilityID abilityID in GetAllowedAbilities(character.GetID()))
        {
            StartCoroutine(GenerateAbilityButton(abilityID));
        }
    }

    private IEnumerator GenerateAbilityButton(AbilityID abilityID)
    {
        GameObject ButtonObject = Instantiate(Prefab_Item, Container);
        EncounterAbilityButton abilityButton = ButtonObject.GetComponent<EncounterAbilityButton>();
        yield return new WaitUntil(() => abilityButton != null);
        abilityButton.Populate(abilityID);
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
}
