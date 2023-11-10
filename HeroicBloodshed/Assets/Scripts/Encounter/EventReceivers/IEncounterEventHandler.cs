using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

public interface IEncounterEventHandler 
{
    public IEnumerator Coroutine_EncounterStateUpdate(EncounterState stateID, EncounterModel model);
}
