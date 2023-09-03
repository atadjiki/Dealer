using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static Constants;

public class EncounterCanvas : MonoBehaviour, IEncounter
{
    //elements
    [Header("Elements")]
    [SerializeField] private Image Panel_Fade;
    [SerializeField] private GameObject Panel_Encounter;

    //the containers for prefabs to be spawned in
    [Header("Containers")]
    [SerializeField] private GameObject Panel_PlayerQueue;
    [SerializeField] private GameObject Panel_EnemyQueue;

    [Header("Prefabs")]
    [SerializeField] private GameObject Prefab_Portrait;
    [SerializeField] private GameObject Prefab_EnemyText;

    public IEnumerator HandleInit()
    {
        Panel_Encounter.SetActive(false);
        yield return Coroutine_PerformFadeToBlack();
    }

    public void EncounterStateCallback(Encounter encounter)
    {
        StartCoroutine(Coroutine_EncounterStateCallback(encounter));
    }

    private IEnumerator Coroutine_EncounterStateCallback(Encounter encounter)
    {
        EncounterState state = encounter.GetState();

        switch (state)
        {
            case EncounterState.SETUP_COMPLETE:
            {
                yield return Coroutine_PerformFadeToClear();
                break;
            }
            case EncounterState.WAIT_FOR_PLAYER_INPUT:
            {
                PopulateQueues(encounter);
                break;
            }
            case EncounterState.DONE:
            {
                encounter.OnStateChanged -= this.EncounterStateCallback;
                Destroy(this.gameObject);
                yield break;
            }
            default:
            {
                HideAll();
                break;
            }
        }

        yield return null;
    }

    private IEnumerator Coroutine_PerformFadeToBlack()
    {
        StopCoroutine("Coroutine_PerformFadeToClear");
        yield return Coroutine_PerformFadeBetween(Color.clear, Color.black, 0.25f);
    }

    private IEnumerator Coroutine_PerformFadeToClear()
    {
        StopCoroutine("Coroutine_PerformFadeToClear");
        yield return Coroutine_PerformFadeBetween(Color.black, Color.clear, 0.25f);
    }

    private IEnumerator Coroutine_PerformFadeBetween(Color to, Color from, float duration)
    {
        Debug.Log("Fade from " + to.ToString() + " to " + from.ToString());
        Panel_Fade.color = to;

        float currentTime = 0;

        while(currentTime < duration)
        {
            currentTime += Time.deltaTime;
            Panel_Fade.color = Color.Lerp(to, from, currentTime/duration);
            yield return null;
        }
    }

    private void PopulateQueues(Encounter encounter)
    {
        Panel_Encounter.SetActive(true);
    }

    private void HideAll()
    {
        Panel_Encounter.SetActive(false);
    }
}
