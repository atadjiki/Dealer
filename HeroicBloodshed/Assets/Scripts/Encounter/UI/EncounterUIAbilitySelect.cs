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

        ResourceRequest resourceRequest = GetTexture(abilityID);

        yield return new WaitUntil(() => resourceRequest.isDone);

        Texture2D icon = (Texture2D)resourceRequest.asset;

        abilityButton.Populate(abilityID, icon, character);
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
}
