using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static Constants;
public class EncounterEventBanner : EncounterUIElement
{
    [SerializeField] private TextMeshProUGUI Text_EventDetail;

    public override void Populate(EncounterModel model)
    {
        CharacterComponent caster = model.GetCurrentCharacter();
        CharacterComponent target = model.GetActiveTarget();
        AbilityID abilityID = model.GetActiveAbility();

        string eventString = "";

        eventString += GetDisplayString(caster.GetID()) + " performs ";
        eventString += GetDisplayString(abilityID);

        if(target != null)
        {
            eventString += " on " + GetDisplayString(target.GetID());
        }

        Text_EventDetail.text = eventString;
    }

    public override void HandleStateUpdate(EncounterState stateID, EncounterModel model)
    {
        switch(stateID)
        {
            case EncounterState.PERFORM_ACTION:
                Populate(model);
                Show();
                break;
            default:
                Hide();
                break;
        }
    }
}
