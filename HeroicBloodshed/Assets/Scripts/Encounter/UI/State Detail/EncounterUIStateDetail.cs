using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static Constants;

public class EncounterUIStateDetail : EncounterUIElement
{
    [SerializeField] private TextMeshProUGUI Text_Detail;

    public override void Populate(EncounterModel model)
    {
        Text_Detail.text = GetDisplayString(model.GetState());
    }
}
