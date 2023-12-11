using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Constants;

public class EncounterUITeamBanner : EncounterUIElement
{
    [SerializeField] private Image Panel_Backing;
    [SerializeField] private TextMeshProUGUI Text_Team;

    public override void Populate(EncounterModel model)
    {
        Panel_Backing.color = GetColor(model.GetCurrentTeam(), 0.25f);
        Text_Team.text = (model.GetCurrentTeam() + " turn").ToLower();
    }

    public override void HandleStateUpdate(EncounterState stateID, EncounterModel model)
    {
        switch (stateID)
        {
            case EncounterState.TEAM_UPDATED:
                {
                    if (model.IsCurrentTeamCPU())
                    {
                        Populate(model);
                        Show();
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
}
