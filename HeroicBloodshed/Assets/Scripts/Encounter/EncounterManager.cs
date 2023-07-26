using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine;

public class EncounterManager : MonoBehaviour
{
    [SerializeField] private Encounter CurrentEncounter;

    private void Awake()
    {
        if(CurrentEncounter != null)
        {
            SetupEncounter(CurrentEncounter);
        }
    }

    public void SetupEncounter(Encounter encounter)
    {
        StartCoroutine(Coroutine_SetupEncounter(encounter));
    }

    private IEnumerator Coroutine_SetupEncounter(Encounter encounter)
    {
        Debug.Log("Setting Up Encounter " + encounter.gameObject.name);

        encounter.SpawnCharacters();

        yield return new WaitUntil( () => encounter.GetCurrentState() == EncounterConstants.State.ACTIVE);

        yield return null;
    }
}
