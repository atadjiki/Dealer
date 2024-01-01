using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using static Constants;

public class EncounterUICurrentCharacter : EncounterUIElement
{
    [Header("Elements")]
    [SerializeField] private TextMeshProUGUI Text_Name;
    [SerializeField] private TextMeshProUGUI Text_Weapon;

    [Header("Containers")]
    [SerializeField] private GameObject Container_Ammo;

    [Header("Prefabs")]
    [SerializeField] private GameObject Prefab_AmmoItem;

    public override void Populate(EncounterModel model)
    {
        CharacterComponent currentCharacter = model.GetCurrentCharacter();

        Text_Name.text = GetDisplayString(currentCharacter.GetID());

        Text_Weapon.text = GetDisplayString(currentCharacter.GetWeaponID());

        int ammoCount = 0; //TODO currentCharacter.GetRemainingAmmo();

        for(int i = 0; i < ammoCount; i++)
        {
            Instantiate(Prefab_AmmoItem, Container_Ammo.transform);
        }
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

    public override void Hide()
    {
        base.Hide();

        UIHelper.ClearTransformChildren(Container_Ammo.transform);
    }
}
