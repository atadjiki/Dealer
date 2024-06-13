using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

public class EncounterCharacterHighlight : EncounterEventHandler
{
    [SerializeField] private GameObject Quad;

    private void Update()
    {
        Quad.transform.eulerAngles = new Vector3(90, 0, 0);
    }

    protected override void OnStateChangedCallback(EncounterStateData state)
    {
        if (state.GetCurrentState() == EncounterState.PERFORM_ACTION)
        {
            Destroy(this.gameObject);
        }
    }
}
