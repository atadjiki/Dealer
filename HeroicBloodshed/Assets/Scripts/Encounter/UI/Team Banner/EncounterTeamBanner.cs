using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Constants;

public class EncounterTeamBanner : EncounterCanvasElement
{
    [SerializeField] private Image Panel_Backing;
    [SerializeField] private TextMeshProUGUI Text_Team;

    public override void Populate(EncounterModel model)
    {
        Panel_Backing.color = GetColorByTeam(model.GetCurrentTeam(), 0.25f);
        Text_Team.text = (model.GetCurrentTeam() + " turn").ToLower();
    }

    public override void Clear()
    {
        Panel_Backing.color = Color.clear;
        Text_Team.text = string.Empty;
    }
}
