using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

public class EnvironmentDestinationHighlight : EncounterEventHandler
{
    protected override void OnStateChangedCallback(EncounterStateData state)
    {
         Destroy(this.gameObject);
    }
}
