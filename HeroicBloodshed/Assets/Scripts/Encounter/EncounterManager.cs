using System.Collections;
using System.Collections.Generic;
using static Constants;
using UnityEngine;

public class EncounterManager : MonoBehaviour
{
    [SerializeField] private Encounter CurrentEncounter;

    private void Awake()
    {
    }

    public void SetupEncounter(Encounter encounter)
    {
        StartCoroutine(Coroutine_SetupEncounter(encounter));
    }

    private IEnumerator Coroutine_SetupEncounter(Encounter encounter)
    {
        Debug.Log("Setting Up Encounter " + encounter.gameObject.name);

        //encounter.SpawnCharacters();

        yield return null;
    }
}
