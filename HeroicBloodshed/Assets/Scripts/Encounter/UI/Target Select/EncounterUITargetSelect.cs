using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

public class EncounterUITargetSelect : EncounterUIElement
{
    [SerializeField] protected Transform Container;

    [SerializeField] protected GameObject Prefab_Item;

    public override void Populate(EncounterModel model)
    {
        //add a portrait for each character in the enemy team
        foreach (CharacterComponent character in model.GetAllCharactersInTeam(TeamID.Enemy))
        {
            StartCoroutine(GenerateTargetButton(character));
        }
    }

    public override void HandleStateUpdate(EncounterState stateID, EncounterModel model)
    {
        switch (stateID)
        {
            case EncounterState.CHOOSE_TARGET:
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

    private IEnumerator GenerateTargetButton(CharacterComponent character)
    {
        GameObject ButtonObject = Instantiate(Prefab_Item, Container);
        EncounterTargetButton targetButton = ButtonObject.GetComponent<EncounterTargetButton>();
        yield return new WaitUntil(() => targetButton != null);
        targetButton.Populate(character);
        targetButton.onClick.AddListener(() => OnTargetButtonClicked(targetButton));
    }

    private void OnTargetButtonClicked(EncounterTargetButton button)
    {
        if (EncounterManager.Instance != null)
        {
            EncounterManager.Instance.SelectTarget(button.GetCharacter());
        }
    }

    public override void Hide()
    {
        base.Hide();

        UIHelper.ClearTransformChildren(Container);
    }
}
