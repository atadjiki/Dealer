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

    [SerializeField] private float DestroyAfter = 3.0f;

    public override void Populate(EncounterModel model)
    {
        Panel_Backing.color = GetColorByTeam(model.GetCurrentTeam(), 0.25f);
        Text_Team.text = (model.GetCurrentTeam() + " turn").ToLower();

        StartCoroutine(DestroyAfterSeconds(DestroyAfter));
    }

    private IEnumerator DestroyAfterSeconds(float duration)
    {
        yield return new WaitForSeconds(duration);
        GameObject.Destroy(this.gameObject);
    }
}
