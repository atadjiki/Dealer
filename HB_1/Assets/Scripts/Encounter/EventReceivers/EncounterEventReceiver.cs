using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

public abstract class EncounterEventReceiver : MonoBehaviour
{
    public abstract IEnumerator Coroutine_Init(EncounterModel model);

    public abstract IEnumerator Coroutine_StateUpdate(EncounterState stateID, EncounterModel model);
}
