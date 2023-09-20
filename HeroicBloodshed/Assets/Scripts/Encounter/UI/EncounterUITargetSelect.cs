using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static Constants;

public class EncounterUITargetSelect : EncounterUIElement
{
    [Header("Variables")]
    [SerializeField] private TextMeshProUGUI Text_Ability;
    [SerializeField] private Button Button_Back;

    [Header("Container")]
    [SerializeField] protected Transform Container;

    [SerializeField] protected GameObject Prefab_Item;

    private void Awake()
    {
        Button_Back.onClick.AddListener(() => OnBackButtonClicked());
    }

    public override void Populate(EncounterModel model)
    {
        Text_Ability.text = GetDisplayString(model.GetActiveAbility());
;
        //add a portrait for each character in the enemy team
        foreach (CharacterComponent character in model.GetAllCharactersInTeam(TeamID.Enemy))
        {
            if(character.IsAlive())
            {
                StartCoroutine(GenerateTargetButton(character));
            }
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

    private void OnBackButtonClicked()
    {
        if(EncounterManager.Instance != null)
        {
            EncounterManager.Instance.OnAbilityCancelled();
        }
    }

    public override void Hide()
    {
        base.Hide();

        UIHelper.ClearTransformChildren(Container);
    }
}
